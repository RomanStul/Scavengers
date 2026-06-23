using System;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Environment.Traps_and_puzzles
{
    public class Barricade : SaveIDDistibutor
    {
        //================================================================CLASSES
        [Serializable]
        private class Trigger
        {
            public GameObject triggerObject;
            public bool isSet;
        }
        
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private UnityEvent activateBarricade, deactivateBarricade;
        [SerializeField] private UnityEvent alreadyActivatedEvent;
        
        [SerializeField] private Trigger[] triggers;

        [SerializeField] private bool isPermanentOpen;

        [SerializeField] private bool needAllTriggers = true;
        //================================================================GETTER SETTER
        public bool GetActivation()
        {
            return isActivated;
        }
        //================================================================FUNCTIONALITY
        private bool isActivated = false;

        private void Awake()
        {
            if (isPermanentOpen && DestructionManager.instance.CheckForBarricade(Id))
            {
                activateBarricade?.Invoke();
                isActivated = true;
            }
        }

        public void TriggerActivated(GameObject trigger)
        {
            
            bool allSet = true;
            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i].triggerObject == trigger || triggers[i].triggerObject == null)
                {
                    triggers[i].isSet = true;
                    if (!needAllTriggers)
                    {
                        ActivateBarricade();
                    }
                }
                allSet = allSet && triggers[i].isSet;
            }

            if (allSet)
            {
                ActivateBarricade();
            }
        }

        public void TriggerReset(GameObject trigger)
        {
            if (isPermanentOpen)
            {
                return;
            }
            if (needAllTriggers)
            {
                deactivateBarricade?.Invoke();
                isActivated = false;
                return;
            }

            bool atLeastOne = false;
            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i].triggerObject == trigger)
                {
                    triggers[i].isSet = false;
                }
                atLeastOne |= triggers[i].isSet;
            }

            if (!atLeastOne)
            {
                deactivateBarricade?.Invoke();
                isActivated = false;
            }
        }

        private void ActivateBarricade()
        {
            activateBarricade?.Invoke();
            if (isPermanentOpen)
            {
                DestructionManager.instance.AddBarricade(Id);
                isActivated = true;
            }
        }
    }
}
