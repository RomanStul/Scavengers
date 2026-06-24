using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interactions;
using Menu;
using Milestones;
using Player.Module.Upgrades;
using Player.UI;
using story;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment = Entities.Environment.Environment;

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

        [Serializable]
        public class TimerConstants
        {
            public float baseTimerLenght;
            public float maxTimerLenght;
            public float increasePerDay;
            public int firstIncreaseDay;
        }
        
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private List<bool> availableInteractions;
        [SerializeField] private TimerConstants timerConstants;
        
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
                CalculateCurrentFullTimer();
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFullTimer, UIController.BarsNames.Timer, true);
                if ((!Mathf.Approximately(value, currentFullTimer) || SceneManager.GetActiveScene().name != "OutpostScene") && !timerIsRunning)
                {
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
        private bool inventoryFullWarned = false;
        private float currentFullTimer = 0;

        public override void ApplyUpgrades()
        {
            bool passkey = ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Portal_passkey);
                availableInteractions[(int)InteractionType.Portal] = passkey && timerValue < -50 || availableInteractions[(int)InteractionType.Portal];
                availableInteractions[(int)InteractionType.Resources] = passkey;
                availableInteractions[(int)InteractionType.Repair] = passkey;

            bool toolsUnlock = ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.ToolsUnlock);
                availableInteractions[(int)InteractionType.Items] = toolsUnlock;

            bool endOfDay = ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.EndOfDay);
                availableInteractions[(int)InteractionType.EndOfDay] = endOfDay;
        }

        private void SetUpInteractableEntity(Interactable interaction)
        {
            if (!availableInteractions[(int)interaction.InteractionType])
            {
                return;
            }
            if (currentInteractableEntity != null)
            {
                currentInteractableEntity.Activate(false, ModuleRef);
            }
            currentInteractableEntity = interaction;
            currentInteractableEntity.Activate(true, ModuleRef);
        }

        private float CalculateCurrentFullTimer()
        {
            currentFullTimer = Mathf.Min(timerConstants.baseTimerLenght + timerConstants.increasePerDay * (StoryManager.instance.GetDayNumber() - timerConstants.firstIncreaseDay), timerConstants.maxTimerLenght);
            return currentFullTimer * SavesManager.Instance.GetDifficulty().dayLengthMultiplier;
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

                    if (ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).GetFreeSpace() == 0 && !inventoryFullWarned && SceneManager.GetActiveScene().name == "OutpostScene")
                    {
                        inventoryFullWarned = true;
                        ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).ShowMonologHelp("I should sell something first");
                        return;
                    }
                    if(!timerIsRunning)
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

                if (currentInteractableEntity.InteractionType == InteractionType.Resources)
                {
                    inventoryFullWarned = false;
                }
                currentInteractableEntity.Use();
            }
        }


        public void ResetTimer()
        {
            timerValue = CalculateCurrentFullTimer();
            availableInteractions[(int)InteractionType.Portal] = true;
            timerIsRunning = false;
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)timerValue, UIController.BarsNames.Timer, true);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)timerValue, UIController.BarsNames.Timer);
        }

        private IEnumerator TimerCountdown()
        {
            if (timerValue < -50)
            {
                ResetTimer();
            }

            timerIsRunning = true;
            float nextThreshold = timerValue * 0.75f;
            while (timerValue >  0 && timerIsRunning)
            {
                timerValue -= Time.deltaTime * Environment.instance.timerMultiplier;
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)timerValue, UIController.BarsNames.Timer, false);
                if (timerValue < nextThreshold)
                {
                    nextThreshold = nextThreshold - timerConstants.baseTimerLenght * 0.25f;
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

            if (shouldInvokeTimerMilestone)
            {
                SceneMilestoneManager.currentInstance.CompletedMilestone(new GlobalMilestoneManager.Milestone(GlobalMilestoneManager.MilestoneAction.TimerRanOut, 1));
            }
            ModuleRef.Evacuate();
        }
    }
}
