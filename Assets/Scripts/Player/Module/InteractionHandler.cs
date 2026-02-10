using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interactions;
using Player.Module.Upgrades;
using Player.UI;
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
        [SerializeField] private float interactionTimerLength;
        
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

        private float timerValue = -99;
        private bool timerIsRunning = false;

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

            if (timerValue < -50)
            {
                ResetTimer();
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
                if (currentInteractableEntity.InteractionType == InteractionType.Portal && !timerIsRunning)
                {
                    StartCoroutine(TimerCountdown());
                }
                currentInteractableEntity.Use();
            }
        }


        public void ResetTimer()
        {
            timerValue = interactionTimerLength;
            availableInteractions.Add(InteractionType.Portal);
            timerIsRunning = false;
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)interactionTimerLength, UIController.BarsNames.Timer, true);
        }

        private IEnumerator TimerCountdown()
        {
            timerValue = interactionTimerLength;
            timerIsRunning = true;
            float nextThreshold = interactionTimerLength * 0.75f;
            while (timerValue > 0)
            {
                timerValue -= Time.deltaTime;
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)timerValue, UIController.BarsNames.Timer, false);
                if (timerValue < nextThreshold)
                {
                    nextThreshold = nextThreshold - interactionTimerLength * 0.25f;
                    ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).PlaySound(ModuleSounds.SoundName.Timer, transform);
                }
                yield return null;
            }

            availableInteractions.Remove(InteractionType.Portal);
            if (currentInteractableEntity != null && currentInteractableEntity.InteractionType == InteractionType.Portal)
            {
                currentInteractableEntity.Activate(false, ModuleRef);
                currentInteractableEntity = null;
            }
            
            ModuleRef.Evacuate();
        }
    }
}
