using System.Collections;
using HelpScripts;
using Menu;
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
        }

        public void IncrementDay()
        {
            StoryManager.instance.IncrementDay();
        }

        public void EndBranchWithDeath()
        {
            SavesManager.Instance.EndBranchWithDeath();
        }

        
        //Decides which animation to play when end of day is triggered
        public void StartEndOfDayAnimation()
        {
            //ASK story manager for what to do in object with all the info

            if (StoryManager.instance.GetDayNumber() == 1 || ((Storage)ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript)).Currency < 0)
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
        
        public void PlayInterrogation()
        {
            //Reference story manager object about day end
            
            if (StoryManager.instance.GetDayNumber() == 1)
            {
                interrogationWindow.Write(Interrogation.InterrogationName.Day0, ModuleRef);
                gameOverEndOfDay = false;
                return;
            }

            if (ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).Currency < 0)
            {
                interrogationWindow.Write(Interrogation.InterrogationName.FailedToPay, ModuleRef);
                gameOverEndOfDay = true;
                return;
            }
        }

        public void PlayNextInterrogationAnimation()
        {
            //Reference story manager object about day end
            if (gameOverEndOfDay)
            {
                ModuleRef.moduleAnimator.SetTrigger("gameOver");
            }
            else
            {
                interrogationWindow.ShowButtons();
            }
            
        }
        
    }
}
