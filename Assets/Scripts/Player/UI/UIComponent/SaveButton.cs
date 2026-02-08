using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player.UI.UIComponent
{
    public class SaveButton : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private TextMeshProUGUI modifiedDate;
        [SerializeField] private Button buttonComponent;
        [SerializeField] private int branch = -1;
        [SerializeField] private int day = -1;
        [SerializeField] private Image previousImageConnector;
        [SerializeField] private Image branchConnector, branchConnectorExtender;
        
        //================================================================GETTER SETTER
        
        public void SetModifiedDate(string modifiedDateText)
        {
            this.modifiedDate.text = modifiedDateText;
        }

        public void SetSaveInfo(int branchId, int branchDayStart, int dayOffset, int sourceOffset)
        { 
            branch = branchId;
            day = branchDayStart + dayOffset;
            if (dayOffset != 0)
            {
                previousImageConnector.enabled = true;
            }
            else
            {
                if (branch != 0)
                {
                    branchConnector.enabled = true;
                    for (int i = 1; i < sourceOffset; i++)
                    {
                        RectTransform extender = (RectTransform)(Instantiate(branchConnectorExtender, branchConnector.transform).transform);
                        extender.anchoredPosition += new Vector2(0, extender.rect.height * i - 1);
                    }
                }
            }
            
        }
        //================================================================FUNCTIONALITY
        


        public void PlaySave()
        {
            SavesManager.Instance.PlaySave(branch, day);
        }

        public void Deactivate()
        {
            buttonComponent.interactable = false;
        }
        
    }
}
