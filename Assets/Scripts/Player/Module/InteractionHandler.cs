using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Environment;
using Entities.Interactions;
using Milestones;
using Player.Module.Upgrades;
using Player.UI;
using story;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        [SerializeField] private List<bool> availableInteractions;
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

        public float TimerValue
        {
            get => timerValue;
            set
            {
                timerValue = value;
                if ((!Mathf.Approximately(value, interactionTimerLength) || SceneManager.GetActiveScene().name != "OutpostScene") && !timerIsRunning)
                {
                    timerIsRunning = true;
                    StartCoroutine(TimerCountdown());
                }
            }
        }
        
        //================================================================FUNCTIONALITY
        private bool shouldInvokeTimerMilestone = true;
        private Interactable currentInteractableEntity;

        private float timerValue = -99;
        private bool timerIsRunning = false;
        private bool endOfDayWarned = false;

        public override void ApplyUpgrades()
        {
            if (ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Portal_passkey))
            {
                availableInteractions[(int)InteractionType.Portal] = true;
                availableInteractions[(int)InteractionType.Resources] = true;
                availableInteractions[(int)InteractionType.Repair] = true;
            }

            if (ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.ToolsUnlock))
            {
                availableInteractions[(int)InteractionType.Items] = true;
            }

            if (ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.EndOfDay))
            {
                availableInteractions[(int)InteractionType.EndOfDay] = true;
            }
        }

        private void SetUpInteractableEntity(Interactable interaction)
        {
            if (!availableInteractions[(int)interaction.InteractionType])
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
                if (currentInteractableEntity.InteractionType == InteractionType.Portal )
                {
                    if (ModuleRef.GetScript<Entities.HealthBar>(Module.ScriptNames.HealthBarScript).GetHealth() <= 0)
                    {
                        ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).ShowMonologHelp("I should repair first");
                        return;
                    }

                    if (ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).GetFuel() <= 0)
                    {
                        ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).ShowMonologHelp("I should refuel first");
                        return;
                    }
                    if(timerIsRunning)
                        StartCoroutine(TimerCountdown());
                }

                if (currentInteractableEntity.InteractionType == InteractionType.EndOfDay)
                {
                    if (ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).Currency < 0 && !endOfDayWarned)
                    {
                        ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).ShowMonologHelp("I should sell something to mee the quota");
                        endOfDayWarned = true;
                        return;
                    }
                    shouldInvokeTimerMilestone = false;
                    endOfDayWarned = false;
                    ResetTimer();
                    
                }
                currentInteractableEntity.Use();
            }
        }


        public void ResetTimer()
        {
            timerValue = interactionTimerLength;
            availableInteractions[(int)InteractionType.Portal] = true;
            timerIsRunning = false;
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)interactionTimerLength, UIController.BarsNames.Timer);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)interactionTimerLength, UIController.BarsNames.Timer, true);
        }

        private IEnumerator TimerCountdown()
        {
            if (timerValue < -50)
            {
                ResetTimer();
            }
            timerIsRunning = true;
            float nextThreshold = interactionTimerLength * 0.75f;
            while (timerValue >  0 && timerIsRunning)
            {
                timerValue -= Time.deltaTime * Environment.instance.timerMultiplier;
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)timerValue, UIController.BarsNames.Timer, false);
                if (timerValue < nextThreshold)
                {
                    nextThreshold = nextThreshold - interactionTimerLength * 0.25f;
                    ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).PlaySound(ModuleSounds.SoundName.Timer, transform);
                }
                yield return null;
            }

            availableInteractions[(int)InteractionType.Portal] = false;
            if (currentInteractableEntity != null && currentInteractableEntity.InteractionType == InteractionType.Portal)
            {
                currentInteractableEntity.Activate(false, ModuleRef);
                currentInteractableEntity = null;
            }

            if (StoryManager.instance.GetDayNumber() == 1 && shouldInvokeTimerMilestone)
            {
                SceneMilestoneManager.currentInstance.CompletedMilestone(new GlobalMilestoneManager.Milestone(GlobalMilestoneManager.MilestoneAction.TimerRanOut, 1));
            }
            ModuleRef.Evacuate();
        }
    }
}
