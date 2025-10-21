using System;
using Entities.Environment;
using ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities
{
    public class ItemDropper : Destructible
    {
        //================================================================CLASSES
        [Serializable]
        public class Drop
        {
            public ItemSO item;
            public int amount;
        }

        private static Item _itemPrefab;
        //================================================================EDITOR VARIABLES
        [SerializeField] protected Drop[] drops;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public override void Awake()
        {
            if (DestructionManager.instance.CheckForOre(destructibleId))
            {
                Destroy(gameObject);
                return;
            }
            
            if (_itemPrefab == null)
            {
                _itemPrefab = Resources.Load<Item>("Item");
            }
        }

        public void DropItems()
        {
            if(!respawns) DestructionManager.instance.AddOre(destructibleId);   
            
            foreach (var drop in drops)
            {
                for (int j = 0; j < drop.amount; j++)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), _itemPrefab.transform.position.z);
                    Item spawned = Instantiate(_itemPrefab, transform.position + offset, Quaternion.identity); 
                    spawned.SetItemData(drop.item);
                }
            }
        }
    }
}
