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
        
        public static DestructionManager instance;
        
        //TODO change to check by scenes
        private HashSet<int> oreHashSet = new HashSet<int>(); 
        private HashSet<int> destructiblesSet = new HashSet<int>();
        //================================================================FUNCTIONALITY

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
