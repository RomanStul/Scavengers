using System;
using UnityEngine;

namespace Player.Module
{
    public class Storage : ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private readonly int[] _itemStorage = new int [Enum.GetValues(typeof(ItemSO.Items)).Length];
        
        public void AddItem(Entities.Item item, int amount)
        {
            //TODO make it so that item starts moving toward moudule, despawning on collision
            _itemStorage[(int)item.GetItemData().itemType] += amount;
            Destroy(item.gameObject);
        }
    }
}
