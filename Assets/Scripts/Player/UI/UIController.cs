using UnityEngine;
using Player.Module;
using Player.Module.Upgrades;
using Player.UI.Inventory;
using Player.UI.Upgrades;
using ScriptableObjects.Item;
using Input = Player.Module.Input;

namespace Player.UI
{
    public class UIController : Module.ModuleBaseScript
    {
        public enum BarsNames
        {
            HealthBar,
            FuelBar,
            StorageBar
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
            Repair
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private Bar HealthBar, FuelBar, StorageBar;
        [SerializeField] private Cooldown Dash, Stop, SideDash;
        [SerializeField] private CurrencyDisplay currencyDisplay;

        [SerializeField] private InventoryHandler inventory;
        [SerializeField] private UpgradeWindowController upgradeController;
        [SerializeField] private RepairRefuel.RepairRefuel repairRefuel;
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
            
            upgradeController.SetUpUpgrades(upgradesScript.upgradesObject);

        }

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

        public void DisplayBalance(int balance)
        {
            currencyDisplay.DisplayBalance(balance);
        }

        public void ItemAmountChange(ItemSO item, int amount)
        {
            if (amount > 0)
            {
                inventory.AddItem(item, amount);
            }
        }

        public void ItemAmountChange(int item, int amount)
        {
            if (amount > 0)
            {
                inventory.AddItem(item, amount);
            }
        }

        public void SetStorageCapacity(int capacity)
        {
            inventory.SetStorageCapacity(capacity);
        }

        public void RemoveAllItemsFromInventory()
        {
            inventory.RemoveAllItems();
        }

        public void RemoveItemFromInventory(ItemSO item, int amount)
        {
            inventory.RemoveItem(item, amount);
        }

        public void PassRepairParameters(RepairRefuel.RepairRefuel.RepairWindowParameters repairRefuelParameters)
        {
            repairRefuel.SetParameters(repairRefuelParameters);
        }

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
                    inventory.ToggleInventory(InventoryHandler.WindowTypes.Inventory);
                    currentOppenedWindow = inventory.IsOpened() ? inventory : null;
                    break;
                
                case WindowType.Resources:
                    inventory.ToggleInventory(InventoryHandler.WindowTypes.ResourceShop);
                    currentOppenedWindow = inventory.IsOpened() ? inventory : null;
                    break;
                
                case WindowType.Upgrades:
                    upgradeController.ToggleWindow();
                    currentOppenedWindow = upgradeController.IsOpened() ? upgradeController : null;
                    break;
                
                case WindowType.Repair:
                    repairRefuel.ToggleWindow();
                    currentOppenedWindow = repairRefuel.IsOpened() ? repairRefuel : null;
                    break;
                case WindowType.None:
                    currentOppenedWindow = null;
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
    }
}
