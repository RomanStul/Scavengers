using System;
using System.Collections.Generic;
using ScriptableObjects.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player.UI.Inventory
{
    public class InventoryHandler : MonoBehaviour
    {
        //================================================================CLASSES
        
        public enum WindowTypes
        {
            Closed,
            Inventory,
            ResourceShop
        }

        [Serializable]
        public class ObjectsForType
        {
            public GameObject[] toEnable;
        }
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private ItemFrame itemFramePrefab;
        [SerializeField] private Transform itemFrameContainer;

        [SerializeField] private Text storageCapacityText;

        [SerializeField] private ItemDBSO itemsDB;
        [SerializeField] private ObjectsForType[] objectsForType;

        [SerializeField] private UnityEvent<ItemSO, int> sellEvent;
        //================================================================GETTER SETTER

        public void SetStorageCapacity(int capacity)
        {
            storageCapacity = capacity;
            SetCapacityText();
        }
        
        //================================================================FUNCTIONALITY
    
        private List<ItemFrame> itemFrames = new List<ItemFrame>();
        private int storageCapacity;
        private int storedItems;
        private WindowTypes currentType = WindowTypes.Closed;


        public void ToggleInventory(WindowTypes type)
        {
            if (type == WindowTypes.Closed)
            {
                SetWindowToStyle(type);
                currentType = WindowTypes.Closed;
                transform.gameObject.SetActive(false);
                return;
            }
            
            if (currentType == type)
            {
                SetWindowToStyle(WindowTypes.Closed);
                transform.gameObject.SetActive(false);
                currentType = WindowTypes.Closed;
                return;
            }

            transform.gameObject.SetActive(true);
            SetWindowToStyle(type);
            currentType = type;
        }


        private void SetWindowToStyle(WindowTypes type)
        {
            foreach (var toDisable in objectsForType[(int)currentType].toEnable)
            {
                toDisable.SetActive(false);
            }

            foreach (var toEnable in objectsForType[(int)type].toEnable)
            {
                toEnable.SetActive(true);
            }
        }

        public void AddItem(ItemSO item, int amount)
        {
            storedItems = Mathf.Min(storageCapacity, storedItems + amount);
            SetCapacityText();
            
            int j = 0;
                for (; j < itemFrames.Count && amount > 0; j++)
                {
                    if (itemFrames[j].GetFramedItem().itemType == item.itemType)
                    {
                        int remaining = itemFrames[j].AddToFrame(amount);
                        amount = remaining;
                    }

                    if ((int)item.itemType > (int)itemFrames[j].GetFramedItem().itemType)
                    {
                        amount = CreateFrame(item, amount, j);
                    }
                }

            while (amount > 0)
            {
                amount = CreateFrame(item, amount, j++);
            }
        }


        public void RemoveItem(ItemSO item, int amount)
        {
            for (int i = itemFrames.Count-1; i >= 0 && (int)itemFrames[i].GetFramedItem().itemType > (int)item.itemType && amount > 0; i--)
            {
                if ((int)itemFrames[i].GetFramedItem().itemType < (int)item.itemType)
                {
                    //TODO check if frame was destroyed
                    amount -= itemFrames[i].RemoveFromFrame(amount);
                }
            }

            if (amount > 0)
            {
                throw new Exception("STORED ITEMS MISMATCH: Tried to remove more that is stored");
            }
        }
        

        public void AddItem(int index, int amount)
        {
            AddItem(itemsDB.items[index], amount);
        }

        public void RemoveAllItems()
        {
            storedItems = 0;
            for (int i = 0; i < itemFrames.Count; i++)
            {
                itemFrames[0].transform.gameObject.SetActive(false);
                itemFrames[i].RemoveFromFrame();
            }
            itemFrames.Clear();
            
            SetCapacityText();
        }

        private int CreateFrame(ItemSO item, int amount, int frameIndex)
        {
            ItemFrame inserted = Instantiate(itemFramePrefab, itemFrameContainer);
            itemFrames.Insert(frameIndex, inserted);
            inserted.Initialize(item, amount);
            inserted.transform.SetSiblingIndex(frameIndex);
            return amount - 99;
        }

        private void SetCapacityText()
        {
            storageCapacityText.text = storedItems.ToString() + "/" + storageCapacity.ToString();
        }

        
        //Needed for selling slider to show worth of items to sell
        private int CalculateWorth(ItemSO item = null, int amount = -1)
        {
            if (item == null)
            {
                int total = 0;
                for (int i = 0; i < itemFrames.Count; i++)
                {
                    total += itemFrames[i].GetHeldItemsCount() * itemsDB.items[(int)itemFrames[i].GetFramedItem().itemType].price;
                }
                return total;
            }
            else
            {
                if (amount == -1)
                {
                    //for all
                }
                else
                {
                    //for amount
                }
            }

            return 0;
        }

        public void SellItem(bool all)
        {
            Debug.Log("invoking event");
            sellEvent.Invoke(null, -1);
        }
    }
}
