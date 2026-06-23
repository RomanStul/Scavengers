using System.Collections;
using HelpScripts;
using Menu;
using Milestones;
using Player.UI;
using story;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player.Module
{
    public class ModuleAnimations : ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Interrogation interrogationWindow;
        //================================================================GETTER SETTER


        //================================================================FUNCTIONALITY
        private string sceneName;
        private Vector3 positionToTransfer;
        private Vector3 interactablePosition;

        private bool gameOverEndOfDay = false;
        private bool endingEndOfDay = false;
        


        public void DisplayNews()
        {
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.News);
        }
        
        
        public void StartOfDayAnimationSetup()
        {
            ModuleRef.moveRb.linearVelocity = Vector2.zero;
            ModuleRef.moveRb.angularVelocity = 0f;
            transform.position = new Vector3(-45, 0, 0);
            transform.rotation = Convertor.RotationConversion(new Vector3(1, 0, 0), transform);
            ModuleRef.mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            
            
            (ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript)).SetNewDayNumber(StoryManager.instance.GetDayNumber());
            (ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript)).PayWithCurrency((int)(StoryManager.instance.GetStartOfDayPayment()), true);
        }
        public void StartSceneTransferAnimation(Vector3 position, string scene, Vector3 stopAt)
        {
            sceneName = scene;
            positionToTransfer = position;
            ModuleRef.moduleAnimator.SetTrigger("sceneTransfer");
            interactablePosition = stopAt;
        }

        public void TransferToScene()
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                StartCoroutine(SetPositionAfterTransfer());
                return;
            }
            SceneManager.LoadScene(sceneName);
            StartCoroutine(SetPositionAfterTransfer());

        }

        private IEnumerator SetPositionAfterTransfer()
        {
            yield return null;
            transform.position = positionToTransfer;
            ModuleRef.moveRb.linearVelocity = Vector2.zero;
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetUpMinimap();
        }

        public void IncrementDay()
        {
            StoryManager.instance.IncrementDay();
        }

        public void EndBranchWithDeath()
        {
            //2 because when interrogation is started the day increments and this can be called only after it finishes => 2 == 1
            if(StoryManager.instance.GetDayNumber() != 2)
                SavesManager.Instance.EndBranchWithDeath();
        }

        
        //Decides which animation to play when end of day is triggered
        public void StartEndOfDayAnimation()
        {
            //ASK story manager for what to do in object with all the info

            if (ChooseInterrogationText() != -1)
            {
                //interrogation start -> branches into all other interrogations (game over, day one, some endings)
                ModuleRef.moduleAnimator.SetTrigger("interrogation");
            }
            else
            {
                //Typical end of day transition
                ModuleRef.moduleAnimator.SetTrigger("endOfDay");
            }
        }
        
        private int ChooseInterrogationText()
        {
            //Reference story manager object about day end
            
            if (StoryManager.instance.GetDayNumber() == 1)
            {
                interrogationWindow.SetInterrogationToWrite(Interrogation.InterrogationName.Day0);
                gameOverEndOfDay = false;
                return (int)Interrogation.InterrogationName.Day0;
            }

            if (ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).Currency < 0)
            {
                interrogationWindow.SetInterrogationToWrite(Interrogation.InterrogationName.FailedToPay);
                gameOverEndOfDay = true;
                return (int)Interrogation.InterrogationName.FailedToPay;
            }

            if (GlobalMilestoneManager.instance.UnclaimedMilestones.Contains(new GlobalMilestoneManager.Milestone(GlobalMilestoneManager.MilestoneAction.Entered, 5)))
            {
                interrogationWindow.SetInterrogationToWrite(Interrogation.InterrogationName.FoundRuins);
                gameOverEndOfDay = true;
                endingEndOfDay = true;
                return (int)Interrogation.InterrogationName.FoundRuins;
            }
            else
            {
                Debug.Log("unclaimed count " + GlobalMilestoneManager.instance.UnclaimedMilestones.Count);
                foreach (var VARIABLE in GlobalMilestoneManager.instance.UnclaimedMilestones)
                {
                    Debug.Log(VARIABLE.action + "  " + VARIABLE.originID);
                }
                Debug.Log("not in unclaimed");
            }

            return -1;
        }

        public void WriteInterrogation()
        {
            interrogationWindow.Write(ModuleRef);
        }

        public void PlayNextInterrogationAnimation()
        {
            //Reference story manager object about day end
            if (gameOverEndOfDay)
            {
                if (endingEndOfDay)
                {
                    ModuleRef.moduleAnimator.SetTrigger("ending");
                }
                else
                {
                    ModuleRef.moduleAnimator.SetTrigger("gameOver");
                }
            }
            else
            {
                interrogationWindow.ShowButtons();
            }
            
        }
        
    }
}
