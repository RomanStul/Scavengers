using System;
using ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module
{
    public class Storage : ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private int storageCapacity;
        [SerializeField] private int itemsStored;
        [SerializeField] private int[] itemStorage = new int [Enum.GetValues(typeof(ItemSO.Items)).Length];
        [SerializeField] private int currency = 0;
        [SerializeField] private ItemDBSO itemDBSO;
        //================================================================GETTER SETTER

        public int[] ItemStorage
        {
            get
            {
                return itemStorage;
            }
            set
            {
                itemStorage = value;
                for (int i = 0; i < itemStorage.Length; i++)
                {
                    itemsStored += itemStorage[i];
                    ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).ItemAmountChange(i, itemStorage[i]);
                }
                ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
            }
        }

        public int Currency
        {
            get
            {
                return currency;
            }
            set
            {
                Debug.Log("setting currency");
                currency = value;
                ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).DisplayBalance(currency);
            }
        }

        //================================================================FUNCTIONALITY
        
        public override void ApplyUpgrades()
        {
            //TODO space for storage space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(storageCapacity, UI.UIController.BarsNames.StorageBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).DisplayBalance(currency);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetStorageCapacity(storageCapacity);
            itemStorage = new int [Enum.GetValues(typeof(ItemSO.Items)).Length];
        }
        
        public void PickUpItem(Entities.Item item, int amount)
        {
            //TODO change to have functions to handle different types of items like resources
            if(itemsStored < storageCapacity && item.StartCollecting(transform))
            {
                itemStorage[(int)item.GetItemData().itemType] += amount;
                itemsStored++;
                ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
                ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).ItemAmountChange(item.GetItemData(), amount);
                Destroy(item.gameObject);
            }
        }

        public void RemoveItem(ItemSO item, int amount = -1)
        {
            int toRemove = 0;
            
            if (amount == -1)
            {
                itemsStored -= itemStorage[(int)item.itemType];
                itemStorage[(int)item.itemType] = 0;
                toRemove = itemStorage[(int)item.itemType];
            }
            else
            {
                itemsStored -= amount;
                toRemove = itemStorage[(int)item.itemType];
                itemStorage[(int)item.itemType] -= amount;
                if (itemStorage[(int)item.itemType] <= 0)
                {
                    itemStorage[(int)item.itemType] = 0;
                }
                else
                {
                    toRemove = amount;
                }
                
            }
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).RemoveItemFromInventory(item, toRemove);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
        }


        public void RemoveAllItems()
        {
            itemsStored = 0;
            for (int i = 0; i < itemStorage.Length; i++)
            {
                itemStorage[i] = 0;
            }
            
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).RemoveAllItemsFromInventory();
        }

            
        public void AddCurrency(int amount)
        {
            currency += amount;
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).DisplayBalance(currency);
        }

        
        /**
         * <para>Subtracts amount of currency and returns how much it subtracted.</para>
         * <para>Parameters</para> 
         * <para>int amount (-1 = all)</para>
         * <para>bool overpay (true forces to go into negative to pay whole sum)</para>
         */
        public int PayWithCurrency(int amount = -1, bool overpay = false)
        {
            int toReturn = currency;
            if (amount == -1)
            {
                currency = 0;
            }
            else
            {
                if (overpay)
                {
                   currency -= amount;
                   if (currency > 0)
                   {
                       toReturn = amount;
                   } 
                }
                else
                {
                    if (currency <= 0)
                    {
                        return 0;
                    }
                    toReturn = Math.Min(amount, currency);
                    currency = Math.Max(0, currency - amount);
                }

            }
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).DisplayBalance(currency);
            return toReturn;
        }

        public void SellItems(ItemSO item = null, int amount = -1)
        {
            if (item == null)
            {
                int total = 0;
                for (int i = 0; i < itemStorage.Length; i++)
                {
                    total += itemStorage[i] * itemDBSO.items[i].price;
                }
                AddCurrency(total);
                RemoveAllItems();
            }
            else
            {
                if (amount == -1)
                {
                    AddCurrency(item.price * itemStorage[(int)item.itemType]);
                    
                }
                else
                {
                    AddCurrency(item.price * amount);
                }
                RemoveItem(item, amount);
            }
        }

        public bool HasAtleast(int item, int amount)
        {
            return itemStorage[item] >= amount; 
        }
    }
}
