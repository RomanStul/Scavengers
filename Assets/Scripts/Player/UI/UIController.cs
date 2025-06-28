using UnityEngine;
using Player.Module.Upgrades;
using Player.Module;
using Player.UI.Inventory;
using ScriptableObjects.Item;

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
            Items
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private Bar HealthBar, FuelBar, StorageBar;
        [SerializeField] private Cooldown Dash, Stop, SideDash;
        [SerializeField] private CurrencyDisplay currencyDisplay;

        [SerializeField] private InventoryHandler inventory;
        //================================================================GETTER SETTER

        //================================================================FUNCTIONALITY
        
        private UIWindow currentOpponentWindow;
        
        public override void ApplyUpgrades()
        {
            Dash.transform.gameObject.SetActive(
                    ModuleRef.GetScript<Upgrades>(Module.Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Ups.Dash)
                );
            Stop.transform.gameObject.SetActive(
                ModuleRef.GetScript<Upgrades>(Module.Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Ups.Stop)
            );
            SideDash.transform.gameObject.SetActive(
                ModuleRef.GetScript<Upgrades>(Module.Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Ups.DashSideWays)
            );
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

        public void OpenWindow(WindowType win)
        {
            //TODO close currently opened window
            
            switch (win)
            {
                case WindowType.Inventory:
                    inventory.ToggleInventory(InventoryHandler.WindowTypes.Inventory);
                    currentOpponentWindow = inventory;
                    break;
                
                case WindowType.Resources:
                    inventory.ToggleInventory(InventoryHandler.WindowTypes.ResourceShop);
                    currentOpponentWindow = inventory;
                    break;
            }

            if (currentOpponentWindow.blocksInput)
            {
                //TODO set flag in module ref to block input
            }
        }
    }
}
