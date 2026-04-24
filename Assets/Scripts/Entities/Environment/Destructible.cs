using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Entities.Environment
{
    public class Destructible : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] protected int destructibleId = 1;

        [SerializeField] protected bool respawns = false;

        [SerializeField] private UnityEvent onDestroy;

        [SerializeField] private UnityEvent onDestroyLoad;
        //================================================================GETTER SETTER

        public void SetId()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            destructibleId = Random.Range(0, Int32.MaxValue);
        }

        public bool CheckIfShouldBeDestroyed()
        {
            bool isSaved = DestructionManager.instance.CheckForDestructible(destructibleId);
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
            destructibleId = Random.Range(0, Int32.MaxValue);
        }

        public virtual void Awake()
        {
            CheckIfShouldBeDestroyed();
        }

        public void SaveDestructible()
        {
            DestructionManager.instance.AddDestructible(destructibleId);
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
