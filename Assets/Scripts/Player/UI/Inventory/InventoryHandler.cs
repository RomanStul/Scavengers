using System;
using System.Collections.Generic;
using Player.UI.UIComponent;
using ScriptableObjects.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player.UI.Inventory
{
    public class InventoryHandler : UIWindow
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
        [SerializeField] private UnityEvent<ItemSO, int> dropEvent;


        [SerializeField] private Text itemName;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text sliderCount, totalWorthText;
        [SerializeField] private SliderController countSlider;
        [SerializeField] private GameObject highlightItem;

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
        private ItemFrame currentlySelectedItemFrame;
        private int selectedAmount = 1;


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

            ToggleHighlightedItem();
            transform.gameObject.SetActive(true);
            SetWindowToStyle(type);
            currentType = type;
        }

        public override void CloseWindow()
        {
            ToggleInventory(WindowTypes.Closed);
        }


        public override bool IsOpened()
        {
            return currentType != WindowTypes.Closed;
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
                    if (itemFrames[j] == currentlySelectedItemFrame)
                    {
                        ClickedItem(currentlySelectedItemFrame);
                    }
                    amount = remaining;
                }

                if ((int)item.itemType < (int)itemFrames[j].GetFramedItem().itemType)
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
            storedItems -= amount;
            for (int i = itemFrames.Count - 1; i >= 0 && (int)itemFrames[i].GetFramedItem().itemType >= (int)item.itemType && amount > 0; i--)
            {
                if ((int)itemFrames[i].GetFramedItem().itemType == (int)item.itemType)
                {
                    int inFrame = (int)itemFrames[i].GetHeldItemsCount();
                    
                    int removed = itemFrames[i].RemoveFromFrame(amount);
                    amount -= removed;
                    if (inFrame - removed <= 0)
                    {
                        if (currentlySelectedItemFrame == itemFrames[i])
                        {
                            currentlySelectedItemFrame = null;
                            ToggleHighlightedItem();
                        }
                        Destroy(itemFrames[i].gameObject);
                        itemFrames.RemoveAt(i);
                    }
                    else
                    {
                        if (currentlySelectedItemFrame == itemFrames[i])
                        {
                            ClickedItem(currentlySelectedItemFrame);   
                        }
                    }
                }
            }

            if (amount > 0)
            {
                throw new Exception("STORED ITEMS MISMATCH: Tried to remove more than is stored");
            }
            
            SetCapacityText();
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

            currentlySelectedItemFrame = null;
            ToggleHighlightedItem();
            itemFrames.Clear();

            SetCapacityText();
        }

        private int CreateFrame(ItemSO item, int amount, int frameIndex)
        {
            ItemFrame inserted = Instantiate(itemFramePrefab, itemFrameContainer);
            itemFrames.Insert(frameIndex, inserted);
            inserted.Initialize(item, amount);
            inserted.transform.SetSiblingIndex(frameIndex);
            inserted.SetInventoryHandler(this);
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
                    total += itemFrames[i].GetHeldItemsCount() *
                             itemsDB.items[(int)itemFrames[i].GetFramedItem().itemType].price;
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
                    return itemsDB.items[(int)item.itemType].price * amount;
                }
            }

            return 0;
        }

        public void SellItem(bool all)
        {
            sellEvent.Invoke(null, -1);
        }

        public void ClickedItem(ItemFrame frame)
        {
            itemName.text = frame.GetFramedItem().itemType.ToString().Replace("_", " ");
            itemIcon.sprite = frame.GetFramedItem().image;
            currentlySelectedItemFrame = frame;
            ToggleHighlightedItem();
            countSlider.SetMaxValue(frame.GetHeldItemsCount());
            SetSliderCount(1);
        }

        public void SellSelectedItem(bool all)
        {
            sellEvent.Invoke(currentlySelectedItemFrame.GetFramedItem(), all ? currentlySelectedItemFrame.GetHeldItemsCount() : selectedAmount);
        }

        public void DropSelectedItem(bool all)
        {
            dropEvent.Invoke(currentlySelectedItemFrame.GetFramedItem(), all ? currentlySelectedItemFrame.GetHeldItemsCount(): selectedAmount);
        }

        private void SetSliderCountText()
        {
            sliderCount.text = selectedAmount + "/" + currentlySelectedItemFrame.GetHeldItemsCount();
        }

        private void SetTotalWorthText()
        {
            totalWorthText.text = CalculateWorth(currentlySelectedItemFrame.GetFramedItem(), selectedAmount).ToString();
        }

        public void SetSliderCount(Single value)
        {
            selectedAmount = (int)value;
            SetSliderCountText();
            SetTotalWorthText();
        }


        private void ToggleHighlightedItem()
        {
            highlightItem.SetActive(currentlySelectedItemFrame != null);
        }
}
}
