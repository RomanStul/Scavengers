using System;
using Entities.Environment;
using ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities
{
    public class ItemDropper : MonoBehaviour
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

        [SerializeField] private int dropperID;

        [SerializeField] private bool respawns = false;
        //================================================================GETTER SETTER

        public void SetId()
        {
            dropperID = Random.Range(0, Int32.MaxValue);
        }
        //================================================================FUNCTIONALITY

        private void Reset()
        {
            dropperID = Random.Range(0, Int32.MaxValue);
        }

        private void Awake()
        {
            gameObject.SetActive(!OreManager.instance.CheckForOre(dropperID));
            
            if (_itemPrefab == null)
            {
                _itemPrefab = Resources.Load<Item>("Item");
            }
        }

        public void DropItems()
        {
            if(!respawns) OreManager.instance.AddOre(dropperID);   
            
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
