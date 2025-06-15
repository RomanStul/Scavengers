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
        //================================================================GETTER SETTER
        
        public int[] ItemStorage { get { return itemStorage; } set { itemStorage = value; } }
        public int Currency { get { return currency; } set { currency = value; } }
        
        //================================================================FUNCTIONALITY
        
        public override void ApplyUpgrades()
        {
            //TODO space for storage space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(storageCapacity, UI.UIController.BarsNames.StorageBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
        }
        
        public void PickUpItem(Entities.Item item, int amount)
        {
            //TODO change to have functions to handle different types of items like resources
            if(itemsStored < storageCapacity && item.StartCollecting(transform))
            {
                itemStorage[(int)item.GetItemData().itemType] += amount;
                itemsStored++;
                ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
                Destroy(item.gameObject);
            }
        }

        public int RemoveItem(Entities.Item item, int amount = -1)
        {
            if (amount == -1)
            {
                itemStorage[(int)item.GetItemData().itemType] = 0;
                return itemStorage[(int)item.GetItemData().itemType];
            }
            int toRemove = itemStorage[(int)item.GetItemData().itemType];
            itemStorage[(int)item.GetItemData().itemType] -= amount;
            if (itemStorage[(int)item.GetItemData().itemType] <= 0)
            {
                itemStorage[(int)item.GetItemData().itemType] = 0;
                return toRemove;
            }
            return amount;
        }

        public void AddCurrency(int amount)
        {
            currency += amount;
        }

        public int GetCurrency(int amount = -1)
        {
            if (amount == -1)
            {
                return currency;
            }
            int toReturn = currency;
            currency -= amount;
            if (currency <= 0)
            {
                currency = 0;
                return toReturn;
            }
            return amount;
        }
    }
}
