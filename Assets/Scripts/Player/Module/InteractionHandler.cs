using System.Collections.Generic;
using Entities;
using Entities.Interactions;
using Player.Module.Upgrades;
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
            Portal,
            EndOfDay
        }
        
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private List<InteractionType> availableInteractions;
        
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
            if (ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Portal_passkey))
            {
                availableInteractions.Add(InteractionType.Portal);
                availableInteractions.Add(InteractionType.Resources);
                availableInteractions.Add(InteractionType.Repair);
                availableInteractions.Add(InteractionType.Items);
            }

            if (ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.EndOfDay))
            {
                availableInteractions.Add(InteractionType.EndOfDay);
            }
        }

        private void SetUpInteractableEntity(Interactable interaction)
        {
            if (!availableInteractions.Contains(interaction.InteractionType))
            {
                return;
            }
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
