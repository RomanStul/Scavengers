using System.Collections.Generic;
using Entities.Environment.Traps_and_puzzles;
using JetBrains.Annotations;
using Menu;
using UnityEngine;

namespace Entities.Environment
{
    public class DestructionManager : MonoBehaviour
    {
        //================================================================CLASSES

        public class MovableState
        {
            public Vector2 position;
            public Vector2 velocity;
            public Movable movableRef;
        }
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
        
        public int[] GetDestroyedRespawnOresArray()
        {
            int index = 0;
            int[] ores = new int [dayHashSet.Count];
            
            foreach (int ore in dayHashSet)
            {
                ores[index] = ore;
                index++;
            }

            return ores;
        }

        public int[] GetDestroyedObjectsArray()
        {
            int index = 0;
            int[] objects = new int [destructiblesSet.Count];
            
            foreach (int ore in destructiblesSet)
            {
                objects[index] = ore;
                index++;
            }

            return objects;
        }

        public SavesManager.MovableStateToSave[] GetMovablePositionsArray()
        {
            int i = 0;
            SavesManager.MovableStateToSave[] movableArray = new SavesManager.MovableStateToSave[movablePositions.Count];
            foreach (KeyValuePair<int, MovableState> pair in movablePositions)
            {
                Vector2 positionToSave = pair.Value.position;
                Vector2 velocityToSave = pair.Value.velocity;
                if (pair.Value.movableRef != null)
                {
                    positionToSave = pair.Value.movableRef.GetPosition();
                    velocityToSave = pair.Value.movableRef.GetVelocity();
                }

                movableArray[i] = new SavesManager.MovableStateToSave() { id = pair.Key, position = positionToSave, velocity = velocityToSave };
                i++;
            }

            return movableArray;
        }

        public void SetDestroyedOres([NotNull] int[] ores)
        {
            foreach (int oreId in ores)
            {
                oreHashSet.Add(oreId);
            }
        }
        
        public void SetDestroyedRespawnOres([NotNull] int[] ores)
        {
            foreach (int oreId in ores)
            {
                dayHashSet.Add(oreId);
            }
        }

        public void SetDestroyedObjects([NotNull] int[] objects)
        {
            foreach (int ob in objects)
            {
                destructiblesSet.Add(ob);
            }
        }

        public void SetMovablePositions([NotNull] SavesManager.MovableStateToSave[] movableStateArray)
        {
            for (int i = 0; i < movableStateArray.Length; i++)
            {
                movablePositions.Add(movableStateArray[i].id, new MovableState(){position = movableStateArray[i].position, velocity = movableStateArray[i].velocity});
            }
        }

        //================================================================FUNCTIONALITY
        public static DestructionManager instance;
        
        //TODO change to check by scenes
        private HashSet<int> oreHashSet = new HashSet<int>(); 
        private HashSet<int> destructiblesSet = new HashSet<int>();
        private HashSet<int> dayHashSet = new HashSet<int>();
        private Dictionary<int, MovableState> movablePositions = new Dictionary<int, MovableState>();
        
        
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

        public void AddRespawnOres(int ore)
        {
            dayHashSet.Add(ore);
        }

        public bool CheckForOre(int oreID)
        {
            return oreHashSet.Contains(oreID);
        }

        public bool CheckForRespawnOres(int oreID)
        {
            return dayHashSet.Contains(oreID);
        }

        public void AddDestructible(int destructibleID)
        {
            destructiblesSet.Add(destructibleID);
        }

        public bool CheckForDestructible(int destructibleID)
        {
            return destructiblesSet.Contains(destructibleID);
        }

        public void AddMovable(int id, Vector2 position, Vector2 velocity, Movable mov)
        {
            movablePositions[id] = new MovableState(){position = position, velocity = velocity, movableRef = mov};
        }

        public MovableState GetMovableState(int id)
        {
            if (movablePositions.ContainsKey(id))
            {
                return movablePositions[id];
            }

            return new MovableState(){position = Vector2.positiveInfinity, velocity = Vector2.zero};
        }

        public void ResetDayHashSet()
        {
            dayHashSet.Clear();
        }
        
        
    }
}
