using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Player.UI.UIComponent;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WaitForEndOfFrame = UnityEngine.WaitForEndOfFrame;

namespace Menu
{
    public class PlaySubmenu : MonoBehaviour
    {
        //================================================================CLASSES
        [Serializable]
        public class NewSaveForm
        {
            public Transform parentTransform;
            public InputField saveNameInputField;
            public Button CreateNewSaveButton;
        }
        //================================================================EDITOR VARIABLES

        [FormerlySerializedAs("saveButtonPrefab")] [SerializeField] private SaveFolderButton saveFolderButtonPrefab;

        [SerializeField] private Transform saveButtonScrollView;

        [SerializeField] private SaveButtonsGrid buttonGrid;
        [SerializeField] private SaveButton lastSaveButton;

        [SerializeField] private NewSaveForm saveForm;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public void TestSaveName()
        {
            string newSaveName = saveForm.saveNameInputField.text;
            saveForm.CreateNewSaveButton.interactable = SavesManager.Instance.TestSaveName(newSaveName);
        }

        public void StartNewGame()
        {
            SavesManager.Instance.WriteSaveIntoFile(SavesManager.Instance.CreateSaveObject(null, "OutpostScene", new Vector2(0,0), saveForm.saveNameInputField.text));
        }

        public void CreateSavesButtons()
        {

            List<string> saveNames = SavesManager.Instance.GetSaveNames();
            if (saveButtonScrollView.childCount >= saveNames.Count + 1)
            {
                return;
            }
            foreach (string save in saveNames)
            {
                SaveFolderButton sb = Instantiate(saveFolderButtonPrefab, saveButtonScrollView);
                sb.SetSaveName(save);
                sb.SetMainMenu(this);
            }
        }

        public void CreateSaveTree(string clickedSave)
        {
            SavesManager.SavesStructure structure = SavesManager.Instance.LoadSaveStructure(clickedSave);

            List<SavesManager.SaveBranch> branchList = structure.branches.ToList();
            Stack<Tuple<int, int>> branchesPrinted = new Stack<Tuple<int, int>>(); 
            for (int i = 0; i < structure.branches.Length; i++)
            {
                int bestBranch = -1;
                int j = 0;
                for (; j < branchList.Count; j++)
                {
                    if ((branchList[j].originBranch == branchList[j].id || (branchesPrinted.Count != 0 && branchList[j].originBranch == branchesPrinted.Peek().Item1)) && (bestBranch == -1 || branchList[j].startDay > branchList[bestBranch].startDay))
                    {
                        bestBranch = j;
                    }
                }

                if (bestBranch == -1)
                {
                    i--;
                    branchesPrinted.Pop();
                    continue;
                }
                buttonGrid.CreateRow(branchList[bestBranch].startDay, branchList[bestBranch].totalDays, branchList[bestBranch].id, clickedSave, branchesPrinted.Count > 0 ? i - branchesPrinted.Peek().Item2: 0);
                branchesPrinted.Push(new Tuple<int, int>(bestBranch, i));
                branchList.RemoveAt(bestBranch);
            }
            
            lastSaveButton.SetModifiedDate(File.GetLastWriteTime("saves/" + structure.name + "/" + structure.name).ToString("yy-MM-dd"));
            lastSaveButton.gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
        }

        public void ClearSaveTree()
        {
            buttonGrid.DeleteAll();
        }

        public void HideNewSave()
        {
            saveForm.parentTransform.gameObject.SetActive(false);
        }
    }
}
