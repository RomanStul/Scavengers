using System.Collections;
using HelpScripts;
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
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private string sceneName;
        private Vector3 positionToTransfer;
        private Vector3 interactablePosition;
        private bool shouldBeStopping = true;
        
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

        public void DisplayNews()
        {
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.News);
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
            shouldBeStopping = false;
        }

        public void IncrementDay()
        {
            StoryManager.instance.IncrementDay();
        }
        
        
    }
}
