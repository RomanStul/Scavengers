using System;
using Entities.Environment;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private InputField saveName;

        [SerializeField] private Button startNewGame;

        [SerializeField] private SaveButton saveButtonPrefab;

        [SerializeField] private Transform saveButtonScrollView;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            if (DestructionManager.instance != null)
            {
                Destroy(DestructionManager.instance.gameObject);
            }
        }

        public void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void TestSaveName()
        {
            string newSaveName = saveName.text;
            startNewGame.interactable = SavesManager.Instance.TestSaveName(newSaveName);
        }

        public void StartNewGame()
        {
            SavesManager.Instance.CreateSaveObject(null, "OutpostScene", new Vector2(0,0), saveName.text);
            SavesManager.Instance.WriteSaveIntoFile();
        }

        public void CreateSavesButtons()
        {
            SavesManager.Save[] saves = SavesManager.Instance.GetAllSaves();
            foreach (SavesManager.Save save in saves)
            {
                SaveButton sb = Instantiate(saveButtonPrefab, saveButtonScrollView);
                sb.SetSaveDate(save.DatetimeString);
                sb.SetSaveName(save.Name);
                sb.SetSave(save);
                sb.SetMainMenu(this);
                sb.SetSceneName(save.ModuleData.Scene);
            }
        }
    }
}
