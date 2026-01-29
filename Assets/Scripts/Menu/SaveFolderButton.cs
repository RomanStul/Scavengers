using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SaveFolderButton : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Text saveName;
        //================================================================GETTER SETTER
        public void SetMainMenu(PlaySubmenu menu)
        {
            playSubmenu = menu;
        }

        public void SetSaveName(string name)
        {
            saveName.text = name;
        }

        //================================================================FUNCTIONALITY
        private PlaySubmenu playSubmenu;

        public void Clicked()
        {
            playSubmenu.ClearSaveTree();
            playSubmenu.CreateSaveTree(saveName.text);
            playSubmenu.HideNewSave();
        }
    }
}
