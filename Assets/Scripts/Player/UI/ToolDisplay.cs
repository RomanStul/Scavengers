using ScriptableObjects.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class ToolDisplay : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Image icon, countDisplay;
        [SerializeField] private Sprite[] numberSprites;
        

        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            icon.gameObject.SetActive(false);
            countDisplay.gameObject.SetActive(false);
        }
        
        public void SetTool(ToolSO tool, int count)
        {
            if (count == -1 || tool == null)
            {
                icon.gameObject.SetActive(false);
                countDisplay.gameObject.SetActive(false);
                return;
            }
            
            icon.gameObject.SetActive(true);
            countDisplay.gameObject.SetActive(true);
            icon.sprite = tool.icon;
            countDisplay.sprite = numberSprites[count];
        }
    }
}
