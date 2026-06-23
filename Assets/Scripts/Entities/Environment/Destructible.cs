using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Entities.Environment
{
    public class Destructible : SaveIDDistibutor
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        

        [SerializeField] protected bool respawns = false;

        [SerializeField] private UnityEvent onDestroy;

        [SerializeField] private UnityEvent onDestroyLoad;
        //================================================================GETTER SETTER



        public bool CheckIfShouldBeDestroyed()
        {
            bool isSaved = DestructionManager.instance.CheckForDestructible(Id);
            if (isSaved)
            {
                if (onDestroyLoad.GetPersistentEventCount() == 0)
                {
                    gameObject.SetActive(!isSaved);
                }
                else
                {
                    onDestroyLoad.Invoke();
                }
            }

            return isSaved;
        }
        //================================================================FUNCTIONALITY
        
        private void Reset()
        {
            Id = Random.Range(0, Int32.MaxValue);
        }

        public virtual void Awake()
        {
            CheckIfShouldBeDestroyed();
        }

        public void SaveDestructible()
        {
            DestructionManager.instance.AddDestructible(Id);
        }

        public virtual void Destroy()
        {
            if (!respawns)
            {
                SaveDestructible();
            }

            if (onDestroy.GetPersistentEventCount() == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                onDestroy.Invoke();
            }
        }
    }
}
