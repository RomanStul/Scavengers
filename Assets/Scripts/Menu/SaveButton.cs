using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SaveButton : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Text saveName;
        [SerializeField] private Text saveDate;
        private SavesManager.Save save;
        private string sceneName;
        private Vector2 position;
        //================================================================GETTER SETTER
        public void SetMainMenu(MainMenu menu)
        {
            mainMenu = menu;
        }

        public void SetSceneName(string scene)
        {
            sceneName = scene;
        }

        public void SetSaveName(string name)
        {
            saveName.text = name;
        }

        public void SetSaveDate(string date)
        {
            saveDate.text = date;
        }

        public void SetSave(SavesManager.Save save)
        {
            this.save = save;
        }

        //================================================================FUNCTIONALITY
        private MainMenu mainMenu;

        public void Clicked()
        {
            SavesManager.Instance.CurrentSave = save;
            mainMenu.ChangeScene(sceneName);
        }
    }
}
