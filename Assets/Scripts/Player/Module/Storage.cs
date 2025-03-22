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
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public override void ApplyUpgrades()
        {
            //TODO space for storage space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(storageCapacity, UI.UIController.BarsNames.StorageBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
        }
        
        public void AddItem(Entities.Item item, int amount)
        {
            //TODO change to have functions to handle different types of items like  resources and fuel
            if(itemsStored < storageCapacity && item.StartCollecting(transform))
            {
                itemStorage[(int)item.GetItemData().itemType] += amount;
                itemsStored++;
                ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar(itemsStored, UI.UIController.BarsNames.StorageBar);
                Destroy(item.gameObject);
            }
        }
    }
}
