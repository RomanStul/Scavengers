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
            //TODO make it so that item starts moving toward moudule, despawning on collision
            if (itemsStored < storageCapacity)
            {
                itemStorage[(int)item.GetItemData().itemType] += amount;
                itemsStored++;
                Destroy(item.gameObject);
            }
        }
    }
}
