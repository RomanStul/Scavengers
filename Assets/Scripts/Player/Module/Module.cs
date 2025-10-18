using System;
using System.Collections;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Player.Module
{
    public class Module : MonoBehaviour
    {
        
        //================================================================CLASSES

        public enum ScriptNames
        {
            InputScript,
            MovementScript,
            DrillScript,
            CollisionScript,
            StorageScript,
            HealthBarScript,
            UpgradesScript,
            UpgradeVisualsScript,
            UIControlsScript,
            InteractionScript,
        }
        //================================================================EDITOR VARIABLES

        //public Scripts scripts;
        public Player.Module.BaseClass[] baseScripts;
        public Rigidbody2D moveRb;
        //================================================================GETTER SETTER
        public Rigidbody2D GetMoveRb()
        {
            return moveRb;
        }

        public T GetScript<T>(ScriptNames scriptName) where T : BaseClass
        {
            //TODO make so that order of script either doesn't matter or is fixed to correct one
            return (T)baseScripts[(int)scriptName];
        }
        //================================================================FUNCTIONALITY
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            for (int i = 0; i < baseScripts.Length; i++)
            {
                baseScripts[i].SetModule(this);
            }
            
            LoadFromSave();
        }

        public void ApplyUpgrades()
        {
            for (int i = 0; i < baseScripts.Length; i++)
            {
                baseScripts[i].ApplyUpgrades();
            }
        }

        public void CreateStateObject(string scene, Vector2 position)
        {
            Debug.Log("create state object " + SavesManager.Instance);
            if (SavesManager.Instance != null)
            {
                SavesManager.Instance.CreateSaveObject(this, scene, position);
                SavesManager.Instance.WriteSaveIntoFile();
            }
        }

        private void LoadFromSave()
        {
            if (SavesManager.Instance != null)
            {
                //Loads upgrades then calls Apply upgrades on this which calls apply upgrades on all scripts attached
                GetScript<Upgrades.ModuleUpgrades>(ScriptNames.UpgradesScript).LoadUpgrades(SavesManager.Instance.GetSavedUpgrades());
                //Load values and data after upgrades are applied
                SavesManager.Instance.LoadSaveIntoModule(this);
            }
        }

        public void PrepareForSceneTransfer(Vector3 position)
        {
            moveRb.velocity = Vector2.zero;
            transform.position = position;
        }

        public void Evacuate()
        {
            if (SceneManager.GetActiveScene().name == "OutpostScene")
            {
                return;
            }
            PrepareForSceneTransfer(Vector3.zero);
            GetScript<Storage>(ScriptNames.StorageScript).PayWithCurrency(30, true);
            SceneManager.LoadScene("OutpostScene");
        }
    }
}
