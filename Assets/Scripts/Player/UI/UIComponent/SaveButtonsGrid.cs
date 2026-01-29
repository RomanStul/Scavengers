using System;
using System.Collections.Generic;
using System.IO;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.UIComponent
{
    public class SaveButtonsGrid : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private HorizontalLayoutGroup rowPrefab;
        [SerializeField] private SaveButton saveButtonPrefab;

        [SerializeField] private TextMeshProUGUI dayNumberPrefab;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private List<HorizontalLayoutGroup> rows;
        private int maxDays = 1;
        
        
        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            rows = new List<HorizontalLayoutGroup>();
            rows.Add(Instantiate(rowPrefab, transform));
            rows[0].padding.left = (int)(((RectTransform)saveButtonPrefab.transform).rect.width + rowPrefab.spacing);
            ((RectTransform)rows[0].transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);
        }

        public void CreateRow(int column, int numberOfDays, int branch, string saveName, int sourceOffset)
        {
            rows.Add(Instantiate(rowPrefab, transform));
            (rows[^1]).padding.left = (int)(column * (((RectTransform)saveButtonPrefab.transform).rect.width + rowPrefab.spacing));

            for (int i = maxDays; i < numberOfDays + column; i++)
            {
                TextMeshProUGUI dayNumberText = Instantiate(dayNumberPrefab, rows[0].transform);
                dayNumberText.text = "Day\n" + i;
            }
            maxDays = numberOfDays + column;
            
            for (int i = 0; i < numberOfDays; i++)
            {
                SaveButton sb = Instantiate(saveButtonPrefab, rows[^1].transform);
                sb.SetSaveInfo(branch, column, i, sourceOffset);
                sb.SetModifiedDate(File.GetLastWriteTime(SavesManager.SaveFilePathCompositor(saveName, branch, column + i)).ToString("yy-MM-dd"));
                if (column == 1 && i == 0)
                {
                    sb.Deactivate();
                    sb.SetModifiedDate(File.GetLastWriteTime("saves/" + saveName + "/structure").ToString("yy-MM-dd"));
                }
                
            }
        }

        public void DeleteAll()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            Initialize();
            maxDays = 1;
        }


    }
}
