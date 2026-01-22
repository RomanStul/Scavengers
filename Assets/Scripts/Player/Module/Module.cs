using System;
using System.Collections;
using Menu;
using Milestones;
using Player.UI;
using story;
using UnityEngine;
using UnityEngine.InputSystem;
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
            ToolScript,
            AnimationFunctionsScript,
        }

        [Serializable]
        public class EvacuateSettings
        {
            public int cost;
            public Vector3 evacuatePosition;
            public bool evacuateToSameScene;
            public string sceneName;
            public float costMultiplier = 1;
        }
        //================================================================EDITOR VARIABLES

        //public Scripts scripts;
        public Player.Module.BaseClass[] baseScripts;
        public Rigidbody2D moveRb;
        public PlayerInput playerInput;
        public Animator moduleAnimator;
        public Camera mainCamera;
        [SerializeField] private EvacuateSettings evacuateSettings;
        
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

        public EvacuateSettings GetEvacuateSettings()
        {
            return evacuateSettings;
        }
        //================================================================FUNCTIONALITY
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            GlobalMilestoneManager.instance.ModuleRef = this;
            
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

        public void CreateStateObject(string scene, Vector2 position, string saveName = "", bool isStartOfDay = false)
        {
            if (SavesManager.Instance != null)
            {
                SavesManager.Instance.WriteSaveIntoFile(saveName == ""
                    ? SavesManager.Instance.CreateSaveObject(this, scene, position, isDayStartSave:isStartOfDay)
                    : SavesManager.Instance.CreateSaveObject(this, scene, position, saveName, isStartOfDay));
            }
        }

        private void LoadFromSave()
        {
            if (SavesManager.Instance != null)
            {
                //Loads upgrades then calls Apply upgrades on this which calls apply upgrades on all scripts attached
                if (GetScript<Upgrades.ModuleUpgrades>(ScriptNames.UpgradesScript)
                    .LoadUpgrades(SavesManager.Instance.GetSavedUpgrades()))
                {
                    //Load values and data after upgrades are applied
                    SavesManager.Instance.LoadSaveIntoModule(this);
                    

                }
                else
                {
                    ApplyUpgrades();
                    
                    SavesManager.Instance.WriteSaveIntoFile(SavesManager.Instance.CreateSaveObject(this, SceneManager.GetActiveScene().name, transform.position));
                }

            }
            else
            {
                ApplyUpgrades();
            }
        }

        public void PrepareForSceneTransfer(Vector3 position)
        {
            moveRb.linearVelocity = Vector2.zero;
            transform.position = position;
        }

        public void Evacuate()
        {
            if (SceneManager.GetActiveScene().name == evacuateSettings.sceneName && !evacuateSettings.evacuateToSameScene)
            {
                return;
            }
            PrepareForSceneTransfer(Vector3.zero);
            GetScript<Storage>(ScriptNames.StorageScript).PayWithCurrency((int)(evacuateSettings.cost * evacuateSettings.costMultiplier), true);
            if (SceneManager.GetActiveScene().name == evacuateSettings.sceneName)
            {
                transform.position = evacuateSettings.evacuatePosition;
            }
            else
            {
                SceneManager.LoadScene("OutpostScene");
            }
        }

        public void SaveAndQuit(bool isStartOfDay)
        {
            CreateStateObject(SceneManager.GetActiveScene().name, transform.position, "", isStartOfDay);
            
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
        }

        public void Save(bool isStartOfDay)
        {
            CreateStateObject(SceneManager.GetActiveScene().name, transform.position, "", isStartOfDay);
        }
        
        
        
        
    }
}
