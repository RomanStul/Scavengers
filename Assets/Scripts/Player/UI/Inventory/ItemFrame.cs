using System;
using ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.Inventory
{
    public class ItemFrame : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] private ItemSO framedItem;

        [SerializeField] private Text counter;
        [SerializeField] private Image image;
        //================================================================GETTER SETTER

        public ItemSO GetFramedItem()
        {
            return framedItem;
        }

        public int GetHeldItemsCount()
        {
            return heldItems;
        }
        
        //================================================================FUNCTIONALITY

        private int heldItems = 0; 
        
        
        public void Initialize(ItemSO itemToFrame, int count)
        {
            framedItem = itemToFrame;
            image.sprite = itemToFrame.image;
            counter.text = count.ToString();
            heldItems = count;
        }

        public int AddToFrame(int count)
        {
            int toReturn = count - (99 - heldItems);
            heldItems = Mathf.Min(99, count + heldItems);
            counter.text = heldItems.ToString();
            return toReturn;
        }

        
        //Returns removed items
        public int RemoveFromFrame(int count = -1)
        {
            if (count == -1 || count >= heldItems)
            {
                Destroy(transform.gameObject);
                return heldItems;
            }

            heldItems = heldItems - count;
            counter.text = heldItems.ToString();
            return count;
        }
    }
}
