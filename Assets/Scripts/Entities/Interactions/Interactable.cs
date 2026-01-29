using HelpScripts;
using Player.Module;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Entities.Interactions
{
    public class Interactable : MonoBehaviour
    {
        //================================================================CLASSES
        
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private UnityEvent activateEvent, deactivateEvent;
        [SerializeField] private GameObject useVisualCue;
        [SerializeField] private InteractionHandler.InteractionType interactionType;
        [SerializeField] private bool usedRepeatably;
        [SerializeField] private UnityEvent<Module> onUse;
        [SerializeField] private Transform useModulePosition;
        [SerializeField] private bool stopModule = true;
        
        //================================================================GETTER SETTER
        public InteractionHandler.InteractionType InteractionType => interactionType;
        //================================================================FUNCTIONALITY

        private Module moduleRef;
        private bool used = false;
        
        //Activate = Ready for use when player is nearby

        public void Activate(bool active, Module module)
        {
            if (used && !usedRepeatably && active)
            {
                return;
            }
            moduleRef = module;
            
            if(active)
                activateEvent?.Invoke();
            else
                deactivateEvent?.Invoke();
            
            useVisualCue.SetActive(active);
        }

        public void Use()
        {
            if (used && !usedRepeatably)
            {
                return;
            }
            
            if (!usedRepeatably)
            {
                useVisualCue.SetActive(false);
            }

            used = true;
            onUse.Invoke(moduleRef);

            Vector3 usePosition = useModulePosition != null ? useModulePosition.position : transform.position;
            moduleRef.moveRb.linearVelocity = Vector3.zero;
            
            if(stopModule)
                StartCoroutine(ModuleManipulation.GradualModuleStop(usePosition, moduleRef, () => true));
            
            
            if (!usedRepeatably)
            {
                Activate(false, moduleRef);
            }
        }
    }
}
