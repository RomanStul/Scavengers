using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Entities.Environment
{
    public class DestructionManager : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER

        public int[] GetDestroyedOresArray()
        {
            int index = 0;
            int[] ores = new int [oreHashSet.Count];
            
            foreach (int ore in oreHashSet)
            {
                ores[index] = ore;
                index++;
            }

            return ores;
        }

        public int[] GetDestroyedObjectsArray()
        {
            int index = 0;
            int[] obects = new int [destructiblesSet.Count];
            
            foreach (int ore in destructiblesSet)
            {
                obects[index] = ore;
                index++;
            }

            return obects;
        }

        public void SetDestroyedOres([NotNull] int[] ores)
        {
            foreach (int oreId in ores)
            {
                oreHashSet.Add(oreId);
            }
        }

        public void SetDestroyedObjects([NotNull] int[] objects)
        {
            foreach (int ob in objects)
            {
                destructiblesSet.Add(ob);
            }
        }

        //================================================================FUNCTIONALITY
        public static DestructionManager instance;
        
        //TODO change to check by scenes
        private HashSet<int> oreHashSet = new HashSet<int>(); 
        private HashSet<int> destructiblesSet = new HashSet<int>();
        
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public void AddOre(int ore)
        {
            oreHashSet.Add(ore);
        }

        public bool CheckForOre(int oreID)
        {
            return oreHashSet.Contains(oreID);
        }

        public void AddDestructible(int destructibleID)
        {
            destructiblesSet.Add(destructibleID);
        }

        public bool CheckForDestructible(int destructibleID)
        {
            return destructiblesSet.Contains(destructibleID);
        }
        
        
    }
}
