using System;
using System.Collections.Generic;
using ScriptableObjects.Item;
using Unity.VisualScripting;
using UnityEngine;
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

        [SerializeField] private ItemSO[] itemsDB;
        [SerializeField] private ObjectsForType[] objectsForType;
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
                currentType = WindowTypes.Closed;
                transform.gameObject.SetActive(false);
                return;
            }
            
            if (currentType == type)
            {
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

        public void AddItem(int index, int amount)
        {
            AddItem(itemsDB[index], amount);
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
    }
}
