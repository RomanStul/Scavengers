using UnityEngine;

namespace Player.UI
{
    public class UIWindow : MonoBehaviour
    {
        //================================================================CLASSES

        //================================================================EDITOR VARIABLES

        public bool blocksInput = true;

        //================================================================GETTER SETTER

        
        
        //================================================================FUNCTIONALITY
        
        public virtual void CloseWindow()
        {
            transform.gameObject.SetActive(false);
        }

        public virtual bool ToggleWindow()
        {
            transform.gameObject.SetActive(!transform.gameObject.activeSelf);
            return transform.gameObject.activeSelf;
        }

        public virtual bool IsOpened()
        {
            return transform.gameObject.activeSelf;
        } 
    }
}
