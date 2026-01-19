using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities.Environment;
using Milestones;
using Player.Module;
using Player.Module.Movement;
using Player.Module.Upgrades;
using story;
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
            public MilestonesData MilestoneData;
            public StoryData StoryData;
            public bool isDayStart = false;
        }

        [Serializable]
        public class ModuleData
        {
            public float Health, Fuel;
            public int[] StoredItems;
            public int Currency;
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

        [Serializable]

        public class MilestonesData
        {
            public string[] Scenes;
            public MilestoneList[] Milestones;
        }

        [Serializable]
        public class MilestoneList
        {
            public GlobalMilestoneManager.Milestone[] Milestones;

            public MilestoneList(GlobalMilestoneManager.Milestone[] milestones)
            {
                Milestones = milestones;
            }
        }

        [Serializable]

        public class StoryData
        {
            public int dayNumber;
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
        private bool dirtyLoad = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            saveNames = LoadSaveNames();
        }

        public void WriteSaveIntoFile(Save saveToWrite)
        {
            dirtyLoad = true;
            var json = JsonUtility.ToJson(saveToWrite);
            StreamWriter saveFile = File.CreateText("saves/"+saveToWrite.Name);
            saveFile.Write(json);
            saveFile.Close();
        }

        public Save CreateSaveObject(Module moduleRef, string scene, Vector2 position, string save = "", bool isDayStartSave = false)
        {
            if (save == "")
            {
                save = currentSave.Name;
            }
            currentSave = new Save();
            currentSave.ModuleData = new ModuleData();
            currentSave.EnvironmentData = new EnvironmentData();
            currentSave.MilestoneData = new MilestonesData();
            currentSave.EnvironmentData.DestroyedOres = Array.Empty<int>();
            currentSave.EnvironmentData.DestroyedObjects = Array.Empty<int>();
            currentSave.Name = save;
            currentSave.DatetimeString = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            currentSave.isDayStart = isDayStartSave;

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
            currentSave.ModuleData.Currency = moduleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).Currency;
            currentSave.ModuleData.Upgrades = moduleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).GetUpgrades();
            currentSave.ModuleData.Scene = scene;
            currentSave.ModuleData.Position = position;
        
            //Environment data
            currentSave.EnvironmentData.Day = 0;
            currentSave.EnvironmentData.DestroyedObjects = DestructionManager.instance.GetDestroyedObjectsArray();
            currentSave.EnvironmentData.DestroyedOres = DestructionManager.instance.GetDestroyedOresArray();
            
            //Milestone Data
            
            string[] scenes = new string[GlobalMilestoneManager.instance.CompletedMilestones.Count];
            MilestoneList[] milestones = new MilestoneList[GlobalMilestoneManager.instance.CompletedMilestones.Count];

            int i = 0;
            foreach (var pair in GlobalMilestoneManager.instance.CompletedMilestones)
            {
                scenes[i] = pair.Key;
                milestones[i] = new MilestoneList(pair.Value.ToArray());
                i++;
            }
            currentSave.MilestoneData.Scenes = scenes;
            currentSave.MilestoneData.Milestones = milestones;
            
            //Story Data
            currentSave.StoryData = new StoryData();
            currentSave.StoryData.dayNumber = StoryManager.instance.GetDayNumber();
            
        
            return currentSave;
        }

        public Save[] GetAllSaves()
        {
            if (saves != null && !dirtyLoad)
            {
                return saves;
            }

            dirtyLoad = false;
            saves = new Save[saveNames.Count];
            int index = 0;
        
            foreach (string fileName in saveNames)
            {
                saves[index] = JsonUtility.FromJson<Save>(File.ReadAllText("saves/" + fileName));
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
            moduleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).Currency = currentSave.ModuleData.Currency;
            moduleRef.transform.position = new Vector3(currentSave.ModuleData.Position.x, currentSave.ModuleData.Position.y, 0);

            if (currentSave.isDayStart)
            {
                moduleRef.moduleAnimator.SetTrigger("startOfDay");
            }
        
            DestructionManager.instance.SetDestroyedOres(currentSave.EnvironmentData.DestroyedOres);
            DestructionManager.instance.SetDestroyedObjects(currentSave.EnvironmentData.DestroyedObjects);
            
            StoryManager.instance.LoadDay(currentSave.StoryData.dayNumber);

            
            Dictionary<string, List<GlobalMilestoneManager.Milestone>> listedMilestones = new Dictionary<string, List<GlobalMilestoneManager.Milestone>>();
            for(int i = 0; i < currentSave.MilestoneData.Scenes.Length; i++)
            {
                listedMilestones.Add(currentSave.MilestoneData.Scenes[i], currentSave.MilestoneData.Milestones[i].Milestones.ToList());
            }
            GlobalMilestoneManager.instance.CompletedMilestones = listedMilestones;
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
