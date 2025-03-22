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
        
        public void AddItem(Entities.Item item, int amount)
        {
            //TODO change to have functions to handle different types of items like  resources and fuel
            if(itemsStored < storageCapacity && item.StartCollecting(transform))
            {
                itemStorage[(int)item.GetItemData().itemType] += amount;
                itemsStored++;
                Destroy(item.gameObject);
            }
        }
    }
}
