using Entities;
using Entities.Interactions;
using UnityEngine;

namespace Player.Module
{
    public class InteractionHandler : ModuleBaseScript
    {
        //================================================================CLASSES
        
        public enum InteractionType
        {
            Items,
            Repair,
            Resources,
            Upgrades,
        }
        
        //================================================================EDITOR VARIABLES
        
        //================================================================GETTER SETTER

        public void SetInteractableEntity(Interactable interaction)
        {
            SetUpInteractableEntity(interaction);
        }

        public void ResetInteractableEntity(Interactable interaction)
        {
            if (interaction == currentInteractableEntity)
            {
                currentInteractableEntity.Activate(false, ModuleRef);
                currentInteractableEntity = null;
            }
        }
        
        //================================================================FUNCTIONALITY
        
        private Interactable currentInteractableEntity;

        public override void ApplyUpgrades()
        {
            //TODO modify available interaction types
        }

        private void SetUpInteractableEntity(Interactable interaction)
        {
            //TODO add logic to unlock interactable items with upgrades
            if (currentInteractableEntity != null)
            {
                currentInteractableEntity.Activate(false, ModuleRef);
            }
            currentInteractableEntity = interaction;
            currentInteractableEntity.Activate(true, ModuleRef);
        }

        public void UseEntity()
        {
            if (currentInteractableEntity != null)
            {
                currentInteractableEntity.Use();
            }
        }
    }
}
