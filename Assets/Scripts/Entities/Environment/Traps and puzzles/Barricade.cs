using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Entities.Environment
{
    public class Barricade : MonoBehaviour
    {
        //================================================================CLASSES
        [Serializable]
        private class Trigger
        {
            public GameObject triggerObject;
            public bool isSet;
        }
        
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private UnityEvent activateBarricade;

        [SerializeField] private Trigger[] triggers;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void TriggerActivated(GameObject trigger)
        {
            bool allSet = true;
            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i].triggerObject == trigger || triggers[i].triggerObject == null)
                {
                    triggers[i].isSet = true;
                }
                allSet = allSet && triggers[i].isSet;
            }

            if (allSet)
            {
                activateBarricade?.Invoke();
            }
        }
    }
}
