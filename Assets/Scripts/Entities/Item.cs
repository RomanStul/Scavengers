using UnityEngine;

namespace Entities
{
    public class Item : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected ItemSO itemData;
        //================================================================GETTER SETTER
        public void SetItemData(ItemSO item)
        {
            itemData = item;
            spriteRenderer.sprite = item.image;
        }

        public ItemSO GetItemData()
        {
            return itemData;
        }
        //================================================================FUNCTIONALITY

        
        
        

    }
}
