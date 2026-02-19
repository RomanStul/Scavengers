using System;
using System.Collections;
using UnityEngine;
using Player.Module;
using Player.Module.Upgrades;
using Player.UI.Inventory;
using Player.UI.Tools;
using Player.UI.Upgrades;
using ScriptableObjects.Item;
using ScriptableObjects.Tools;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Input = Player.Module.Input;

namespace Player.UI
{
    public class UIController : Module.ModuleBaseScript
    {
        public enum BarsNames
        {
            HealthBar,
            FuelBar,
            StorageBar,
            Timer
        }

        public enum Cooldowns
        {
            Dash,
            Stop,
            SideDash
        }
        //================================================================CLASSES
        public enum WindowType
        {
            None,
            Inventory,
            Resources,
            Upgrades,
            Items,
            Repair,
            Pause,
            Transmission,
            Help,
            News
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private Bar HealthBar, FuelBar, StorageBar, timer;
        [SerializeField] private Cooldown Dash, Stop, SideDash;
        [SerializeField] private CurrencyDisplay currencyDisplay;
        [SerializeField] private ToolDisplay toolDisplay;
        
        [SerializeField] private TMPro.TextMeshProUGUI StartOfDayText;

        [SerializeField] private UIWindow[] windows;
        //================================================================GETTER SETTER

        //================================================================FUNCTIONALITY
        
        private UIWindow currentOppenedWindow;
        private WindowType currentWindowType;
        
        public override void ApplyUpgrades()
        {
            ModuleUpgrades upgradesScript = ModuleRef.GetScript<ModuleUpgrades>(Module.Module.ScriptNames.UpgradesScript);
            
            Dash.transform.gameObject.SetActive(
                upgradesScript.IsActive(ModuleUpgrades.Ups.Dash)
                );
            Stop.transform.gameObject.SetActive(
                upgradesScript.IsActive(ModuleUpgrades.Ups.Stop)
            );
            SideDash.transform.gameObject.SetActive(
                upgradesScript.IsActive(ModuleUpgrades.Ups.Sideways_Thrust)
            );
            
            //TODO pass upgrades to windows {storage expansion into inventory and so on}
            
            ((UpgradeWindowController)windows[(int)WindowType.Upgrades]).SetUpUpgrades(upgradesScript.upgradesObject);
            
        }

        #region WindowFunctions

        public UIWindow OpenWindow(WindowType win)
                {
                    if(currentOppenedWindow != null)
                        currentOppenedWindow.CloseWindow();
        
                    if (win == currentWindowType)
                    {
                        currentWindowType = WindowType.None;
                        currentOppenedWindow = null;
                        ModuleRef.GetScript<Input>(Module.Module.ScriptNames.InputScript).SetTakeInput(true);
                        return null;
                    }
                    
                    switch (win)
                    {
                        case WindowType.Inventory:
                            InventoryHandler invHand = ((InventoryHandler)windows[(int)win]);
                            invHand.ToggleInventory(InventoryHandler.WindowTypes.Inventory);
                            currentOppenedWindow = invHand.IsOpened() ? invHand : null;
                            break;
                        
                        case WindowType.Resources:
                            InventoryHandler resHand = ((InventoryHandler)windows[(int)win]);
                            resHand.ToggleInventory(InventoryHandler.WindowTypes.ResourceShop);
                            currentOppenedWindow = resHand.IsOpened() ? resHand : null;
                            break;
                        
                        case WindowType.None:
                            currentOppenedWindow = null;
                            break;
                        
                        case WindowType.Help:
                            windows[(int)win].ToggleWindow();
                            currentOppenedWindow = windows[(int)win].IsOpened() ? windows[(int)win] : null;
                            if (currentOppenedWindow != null) StartCoroutine(HideHelp());
                            break;
                        
                        default:
                            windows[(int)win].ToggleWindow();
                            currentOppenedWindow = windows[(int)win].IsOpened() ? windows[(int)win] : null;
                            break;
                    }
                    
                    
        
                    if (currentOppenedWindow != null)
                    {
                        currentWindowType = currentOppenedWindow.IsOpened() ? win : WindowType.None;
                        ModuleRef.GetScript<Input>(Module.Module.ScriptNames.InputScript).SetTakeInput(!currentOppenedWindow.blocksInput);
                    }
                    else
                    {
                        ModuleRef.GetScript<Input>(Module.Module.ScriptNames.InputScript).SetTakeInput(true);
                        currentWindowType = WindowType.None;
                    }
        
                    return currentOppenedWindow;
                }
        
                public void CloseCurrentWindow()
                {
                    OpenWindow(WindowType.None);
                }

                public void CloseSpecificWindow(WindowType windowType)
                {
                    if (windows[(int)windowType].IsOpened())
                    {
                        OpenWindow(WindowType.None);
                    }
                }

        #endregion

        #region Bars

        public void SetBar(int value, BarsNames barName, bool isMax = false)
                {
                    Bar targetBar = null;
        
                    switch (barName)
                    {
                        case BarsNames.HealthBar:
                            targetBar = HealthBar;
                            break;
                        case BarsNames.FuelBar:
                            targetBar = FuelBar;
                            break;
                        case BarsNames.StorageBar:
                            targetBar = StorageBar;
                            break;
                        case BarsNames.Timer:
                            targetBar = timer;
                            break;
                        default:
                            return;
                    }
        
                    if (isMax)
                    {
                        targetBar.SetMaxValue(value);
                    }
                    else
                    {
                        targetBar.SetValue(value);
                    }
                }
        
                public void StartCooldown(float time, Cooldowns cooldownName)
                {
                    Cooldown targetCooldown = null;
        
                    switch (cooldownName)
                    {
                        case Cooldowns.Dash:
                            targetCooldown = Dash;
                            break;
                        case Cooldowns.Stop:
                            targetCooldown = Stop;
                            break;
                        case Cooldowns.SideDash:
                            targetCooldown = SideDash;
                            break;
                        default:
                            return;
                    }
                    
                    targetCooldown.StartCooldown(time);
                }
        
                public void StartDuration(float time, Cooldowns cooldownName = Cooldowns.SideDash)
                {
                    SideDash.StartDuration(time);
                }

        #endregion

        #region NonWindowUI

        public void DisplayBalance(int balance)
                {
                    currencyDisplay.DisplayBalance(balance);
                }
        
                public void ItemAmountChange(ItemSO item, int amount)
                {
                    if (amount > 0)
                    {
                        ((InventoryHandler)windows[(int)WindowType.Inventory]).AddItem(item, amount);
                    }
                }
        
                public void ItemAmountChange(int item, int amount)
                {
                    if (amount > 0)
                    {
                        ((InventoryHandler)windows[(int)WindowType.Inventory]).AddItem(item, amount);
                    }
                }
        
                public void SetStorageCapacity(int capacity)
                {
                    ((InventoryHandler)windows[(int)WindowType.Inventory]).SetStorageCapacity(capacity);
                }
        
                public void RemoveAllItemsFromInventory()
                {
                    ((InventoryHandler)windows[(int)WindowType.Inventory]).RemoveAllItems();
                }
        
                public void RemoveItemFromInventory(ItemSO item, int amount)
                {
                    ((InventoryHandler)windows[(int)WindowType.Inventory]).RemoveItem(item, amount);
                }

                public void SetNewDayNumber(int number)
                {
                    StartOfDayText.text = "Day " + number;
                }

        #endregion


        #region WindowSpecificFunctions

        public void PassRepairParameters(RepairRefuel.RepairRefuel.RepairWindowParameters repairRefuelParameters)
        {
            ((RepairRefuel.RepairRefuel)windows[(int)WindowType.Repair]).SetParameters(repairRefuelParameters);
        }

        public void SetTool(ToolSO tool, int count)
        {
            toolDisplay.SetTool(tool, count);
        }

        public void ShowTransmission(string message)
        {
            ((Transmission)windows[(int)WindowType.Transmission]).WriteMessage(message);
        }

        public void SetHelpWindowMode(HelpDisplay.DisplayModes mode)
        {
            ((HelpDisplay)windows[(int)WindowType.Help]).SetMode(mode);
        }

        private IEnumerator HideHelp()
        {
            yield return new WaitForSeconds(5f);
            CloseSpecificWindow(WindowType.Help);
        }

        public void SetToolCount(ToolSO.ToolType toolType, int count)
        {
            ((ToolsShopWindow)windows[(int)WindowType.Items]).SetToolCount(toolType, count);
        }
            
        #endregion


        
        
       
    }
}
