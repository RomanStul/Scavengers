using System;
using System.Collections.Generic;
using Entities.Environment;
using Milestones;
using Player.UI.UIComponent;
using story;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        
        [SerializeField] private PlaySubmenu playSubmenu;
        //================================================================GETTER SETTER
        
        public PlaySubmenu PlaySubmenu => playSubmenu;
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            //Destroys the global scripts object with all the singletons aside from Saves Manager
            if (DestructionManager.instance != null)
            {
                Destroy(DestructionManager.instance.gameObject);
            }
        }

        public void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }


    }
}
