using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class ItemDropper : MonoBehaviour
    {
        [Serializable]
        public class Drop
        {
            public ItemSO item;
            public int amount;
        }

        private static Item _itemPrefab;
        //================================================================
        [SerializeField] protected Drop[] drops;
        //================================================================
        //================================================================

        private void Awake()
        {
            if (_itemPrefab == null)
            {
                _itemPrefab = Resources.Load<Item>("Item");
            }
        }

        public void DropItems()
        {
            foreach (var drop in drops)
            {
                for (int j = 0; j < drop.amount; j++)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), _itemPrefab.transform.position.z);
                    Item spawned = Instantiate(_itemPrefab, transform.position + offset, Quaternion.identity); 
                    spawned.SetItemData(drop.item);
                }
            }
        }
    }
}
