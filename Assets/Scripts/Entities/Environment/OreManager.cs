using System.Collections.Generic;
using UnityEngine;

namespace Entities.Environment
{
    public class OreManager : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        
        public static OreManager instance;
        
        //TODO change to check by scenes
        private HashSet<int> oreHashSet = new HashSet<int>();
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
        
        
    }
}
