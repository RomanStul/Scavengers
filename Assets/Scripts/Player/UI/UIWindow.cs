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
    }
}
