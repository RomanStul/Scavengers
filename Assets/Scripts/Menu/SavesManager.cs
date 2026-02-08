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
using UnityEngine.SceneManagement;

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

        [Serializable]

        public class SavesStructure
        {
            public string name;
            public SaveBranch[] branches;
        }

        [Serializable]
        public class SaveBranch
        {
            public int id;
            public int originBranch;
            public int startDay;
            public int totalDays;
        }
        //================================================================EDITOR VARIABLES

        //================================================================GETTER SETTER
        public string CurrentSaveName { get { return currentSaveName; } }
    
        public Save CurrentSave { get { return currentSave; } set { currentSave = value; } }

        public bool[] GetSavedUpgrades()
        {
            if (currentSave == null) return null;
            return currentSave.ModuleData.Upgrades;
        }

        public List<string> GetSaveNames()
        {
            return saveNames;
        }
        //================================================================FUNCTIONALITY
    
        public static SavesManager Instance;
        private Save[] saves;
        private bool dirtyLoad = false;
        private string currentSaveName;
        private int currentBranch;
        private Save currentSave;
        private List<string> saveNames;
        private SavesStructure currentSaveStructure;

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
            if (!Directory.Exists("saves/" + saveToWrite.Name))
            {
                CreateNewSaveDirectory(saveToWrite.Name);
            }
            
            if (saveToWrite.isDayStart)
            {
                if (currentSaveStructure.branches[currentBranch].startDay + currentSaveStructure.branches[currentBranch].totalDays > saveToWrite.StoryData.dayNumber)
                {
                    List<SaveBranch> branchesList = currentSaveStructure.branches.ToList();
                    branchesList.Add(new SaveBranch());
                    branchesList[^1].originBranch = currentBranch;
                    branchesList[^1].id = branchesList.Count - 1;
                    branchesList[^1].totalDays = 1;
                    branchesList[^1].startDay = saveToWrite.StoryData.dayNumber;
                    currentBranch = branchesList.Count - 1;
                    currentSaveStructure.branches = branchesList.ToArray();
                }
                else
                {
                    currentSaveStructure.branches[currentBranch].totalDays++;
                }
                WriteSaveStructureToFile(saveToWrite.Name);
            }

            
            
            dirtyLoad = true;
            var json = JsonUtility.ToJson(saveToWrite);
            string savePath = "saves/" + saveToWrite.Name + "/" +saveToWrite.Name;
            if (saveToWrite.isDayStart)
            {
                savePath = SaveFilePathCompositor(saveToWrite.Name, currentBranch, saveToWrite.StoryData.dayNumber);
            }
            StreamWriter saveFile = File.CreateText(savePath);
            saveFile.Write(json);
            saveFile.Close();


                
        }

        public void CreateNewSaveDirectory(string saveName)
        {
            Directory.CreateDirectory(Path.Combine("saves/" + saveName));
            SavesStructure saveStructure = new SavesStructure();
            saveStructure.name = saveName;
            saveStructure.branches = new SaveBranch[1];
            saveStructure.branches[0] = new SaveBranch
            {
                id = 0,
                originBranch = 0,
                startDay = 1,
                totalDays = 1
            };
            currentBranch = 0;
            currentSaveStructure = saveStructure;
            WriteSaveStructureToFile(saveName);
        }

        private void WriteSaveStructureToFile(string saveName)
        {
            string json = JsonUtility.ToJson(currentSaveStructure);
            StreamWriter saveStructureFile = File.CreateText("saves/" + saveName + "/structure");
            saveStructureFile.Write(json);
            saveStructureFile.Close();
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
            saves = new Save[0];
            /*int index = 0;
        
            foreach (string fileName in saveNames)
            {
                saves[index] = JsonUtility.FromJson<Save>(File.ReadAllText("saves/" + fileName));
                index++;
            }*/
        
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
            string[] directories = Directory.GetDirectories("saves/");
            foreach (string directory in directories)
            {
                loadedNames.Add(Path.GetFileName(directory));
            }
            return loadedNames;
        }

        public SavesStructure LoadSaveStructure(string saveName)
        {
            currentSaveStructure = JsonUtility.FromJson<SavesStructure>(File.ReadAllText("saves/" + saveName + "/structure"));
            return currentSaveStructure;
        }

        public void PlaySave(int branch, int day)
        {
            if (currentSaveStructure == null)
            {
                return;
            }

            string currentSavePath;
            if (branch == -1)
            {
                currentSavePath = "saves/" + currentSaveStructure.name + "/" + currentSaveStructure.name;
            }
            else
            {
                currentSavePath = SaveFilePathCompositor(currentSaveStructure.name, branch, day);
            }

            currentSave = JsonUtility.FromJson<Save>(File.ReadAllText(currentSavePath));

            currentBranch = branch;

            SceneManager.LoadScene(currentSave.ModuleData.Scene);
        }

        public static string SaveFilePathCompositor(string saveName, int branch, int day)
        {
            return "saves/" + saveName + "/" + saveName + "_" + branch + "_" + day;
        }
    }
}
