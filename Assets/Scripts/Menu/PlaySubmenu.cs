using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Player.UI.UIComponent;
using TMPro;
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

        [SerializeField] private TMP_Dropdown difficulty;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private string currentOppenedSave = "";
        
        public void TestSaveName()
        {
            string newSaveName = saveForm.saveNameInputField.text;
            try
            {
                Path.GetFullPath(newSaveName);
                saveForm.CreateNewSaveButton.interactable = SavesManager.Instance.TestSaveName(newSaveName);
            }
            catch (ArgumentException)
            {
                saveForm.CreateNewSaveButton.interactable = false;
            }
        }

        public void DeleteSave(string saveName)
        {
            if (currentOppenedSave == saveName)
            {
                ClearSaveTree();
            }
            SavesManager.Instance.DeleteSave(saveName);
        }

        public void StartNewGame()
        {
            SavesManager.Instance.WriteSaveIntoFile(SavesManager.Instance.CreateSaveObject(null, "OutpostScene", new Vector2(0,0), saveForm.saveNameInputField.text), difficulty.options[difficulty.value].text);
            SavesManager.Instance.PlaySave(-1, -1);
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
            currentOppenedSave = clickedSave;
            SavesManager.SavesStructure structure = SavesManager.Instance.LoadSaveStructure(clickedSave);

            List<SavesManager.SaveBranch> branchList = structure.branches.ToList();
            Stack<Tuple<int, int>> branchesPrinted = new Stack<Tuple<int, int>>(); 
            //Go over all branches
            for (int i = 0; i < structure.branches.Length; i++)
            {
                int bestBranch = -1;
                int j = 0;
                //Find best one for this iteration (with already printed removed)
                for (; j < branchList.Count; j++)
                {
                    //Should be origin or be the latest branch of last printed
                    if ((branchList[j].originBranch == branchList[j].id || (branchesPrinted.Count != 0 && branchList[j].originBranch == branchesPrinted.Peek().Item1)) && (bestBranch == -1 || branchList[j].startDay > branchList[bestBranch].startDay))
                    {
                        bestBranch = j;
                    }
                }

                if (bestBranch == -1)
                {
                    //Pop from printed branches to render ones that come from one source, lower i to still render all branches
                    i--;
                    if (branchesPrinted.Count == 0)
                    {
                        break;
                    }
                    branchesPrinted.Pop();
                    continue;
                }
                //Render row of button for this branch + save it as last rendered
                buttonGrid.CreateRow(branchList[bestBranch].startDay, branchList[bestBranch].totalDays, branchList[bestBranch].id, clickedSave, branchesPrinted.Count > 0 ? i - branchesPrinted.Peek().Item2: 0, branchList[bestBranch].endsWithDeath);
                branchesPrinted.Push(new Tuple<int, int>(bestBranch, i));
                branchList.RemoveAt(bestBranch);
            }
            
            lastSaveButton.SetModifiedDate(File.GetLastWriteTime("saves/" + structure.name + "/" + structure.name).ToString("yy-MM-dd"));
            lastSaveButton.SetSaveInfo(structure.lastSaveBranch, -1, 0, 0);
            lastSaveButton.SetConnectorVisibility(false);
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
