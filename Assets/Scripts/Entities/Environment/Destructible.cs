using UnityEngine;
using System;
using UnityEditor;
using Random = UnityEngine.Random;

namespace Entities.Environment
{
    public class Destructible : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] protected int destructibleId = 1;

        [SerializeField] protected bool respawns = false;
        //================================================================GETTER SETTER

        public void SetId()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            destructibleId = Random.Range(0, Int32.MaxValue);
        }
        //================================================================FUNCTIONALITY
        
        private void Reset()
        {
            destructibleId = Random.Range(0, Int32.MaxValue);
        }

        public virtual void Awake()
        {
            gameObject.SetActive(!DestructionManager.instance.CheckForDestructible(destructibleId));
        }

        public virtual void Destroy()
        {
            if (!respawns)
            {
                DestructionManager.instance.AddDestructible(destructibleId);
            }
            
            Destroy(gameObject);
        }
    }
}
