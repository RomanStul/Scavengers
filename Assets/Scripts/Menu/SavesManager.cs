using System;
using System.Collections.Generic;
using System.IO;
using Entities.Environment;
using Player.Module;
using Player.Module.Movement;
using Player.Module.Upgrades;
using UnityEngine;

namespace Menu
{
    public class SavesManager : MonoBehaviour
    {
        //================================================================CLASSES

        [Serializable]
        public class Save
        {
            public string Name;
            public string DatetimeString;
            public ModuleData ModuleData;
            public EnvironmentData EnvironmentData;
            public bool valid = false;
        }

        [Serializable]
        public class ModuleData
        {
            public float Health, Fuel;
            public int[] StoredItems;
            public bool[] Upgrades;
            public string Scene;
            public Vector2 Position;
        }

        [Serializable]
        public class EnvironmentData
        {
            public int Day;
            public int[] DestroyedOres;
            public int[] DestroyedObjects;
            //changes on market
        }
        //================================================================EDITOR VARIABLES
        private string currentSaveName;
        private Save currentSave;
        private List<string> saveNames;
        //================================================================GETTER SETTER
        public string CurrentSaveName { get { return currentSaveName; } }
    
        public Save CurrentSave { get { return currentSave; } set { currentSave = value; } }

        public bool[] GetSavedUpgrades()
        {
            if (currentSave == null) return null;
            return currentSave.ModuleData.Upgrades;
        }
        //================================================================FUNCTIONALITY
    
        public static SavesManager Instance;
        private Save[] saves;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                saveNames = LoadSaveNames();
                DontDestroyOnLoad(gameObject);
            }
        }

        public void WriteSaveIntoFile()
        {
            Debug.Log("saving");
            var json = JsonUtility.ToJson(currentSave);
            StreamWriter saveFile = File.CreateText("saves/"+currentSave.Name);
            saveFile.Write(json);
            saveFile.Close();
        }

        public Save CreateSaveObject(Module moduleRef, string scene, Vector2 position, string save = "")
        {
            if (save == "")
            {
                save = currentSave.Name;
            }
            currentSave = new Save();
            currentSave.ModuleData = new ModuleData();
            currentSave.EnvironmentData = new EnvironmentData();
            currentSave.EnvironmentData.DestroyedOres = Array.Empty<int>();
            currentSave.EnvironmentData.DestroyedObjects = Array.Empty<int>();
            currentSave.Name = save;
            currentSave.DatetimeString = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            if (moduleRef == null)
            {
                currentSave.valid = false;
                saveNames.Add(save);
                return currentSave;
            }
            else
            {
                currentSave.valid = true;
            }
        
            //Module Data
            currentSave.ModuleData.Health = moduleRef.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript).GetHealth();
            currentSave.ModuleData.Fuel = moduleRef.GetScript<Movement>(Module.ScriptNames.MovementScript).GetFuel();
            currentSave.ModuleData.StoredItems = moduleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).ItemStorage;
            currentSave.ModuleData.Upgrades = moduleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).GetUpgrades();
            currentSave.ModuleData.Scene = scene;
            currentSave.ModuleData.Position = position;
        
            //Environment data
            currentSave.EnvironmentData.Day = 0;
            currentSave.EnvironmentData.DestroyedObjects = DestructionManager.instance.GetDestroyedObjectsArray();
            currentSave.EnvironmentData.DestroyedOres = DestructionManager.instance.GetDestroyedOresArray();
        
            return currentSave;
        }

        public Save[] GetAllSaves()
        {
            if (saves != null)
            {
                return saves;
            }
        
            saves = new Save[saveNames.Count];
            int index = 0;
        
            foreach (string name in saveNames)
            {
                saves[index] = JsonUtility.FromJson<Save>(File.ReadAllText("saves/" + name));
                index++;
            }
        
            return saves;
        }

        public void LoadSaveIntoModule(Module moduleRef)
        {
            if (currentSave == null || !currentSave.valid)
            {
                return;
            }
            //load all except upgrades because they are loaded before loading values
            moduleRef.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript).SetHealth(currentSave.ModuleData.Health);
            moduleRef.GetScript<Movement>(Module.ScriptNames.MovementScript).SetFuel(currentSave.ModuleData.Fuel);
            moduleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).ItemStorage = currentSave.ModuleData.StoredItems;
            moduleRef.transform.position = new Vector3(currentSave.ModuleData.Position.x, currentSave.ModuleData.Position.y, 0);
        
            DestructionManager.instance.SetDestroyedOres(currentSave.EnvironmentData.DestroyedOres);
            DestructionManager.instance.SetDestroyedObjects(currentSave.EnvironmentData.DestroyedObjects);
        }

        public bool TestSaveName(string newSaveName)
        {
            foreach (string saveName in saveNames)
            {
                if (saveName == newSaveName)
                {
                    return false;
                }
            }
        
            return true;
        }

        private List<string> LoadSaveNames()
        {
            List<string> loadedNames = new List<string>();
            if (!Directory.Exists("saves/"))
            {
                Directory.CreateDirectory("saves/");
            }
            string[] filePaths = Directory.GetFiles("saves/");
            foreach (string filePath in filePaths)
            {
                loadedNames.Add(Path.GetFileName(filePath));
            }
            return loadedNames;
        }
    }
}
