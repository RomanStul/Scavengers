using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Environment
{
    public class SaveIDDistibutor : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] protected int Id = 1;
        
        //================================================================GETTER SETTER
        public void SetId()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            Id = Random.Range(0, Int32.MaxValue);
        }
        //================================================================FUNCTIONALITY
        
        


    }
}
