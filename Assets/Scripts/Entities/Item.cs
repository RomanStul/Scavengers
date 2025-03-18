using UnityEngine;

namespace Entities
{
    public class Item : MonoBehaviour
    {
        //================================================================
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected ItemSO itemData;
        //================================================================
        //================================================================
        
        
        
        public void SetItemData(ItemSO item)
        {
            itemData = item;
            spriteRenderer.sprite = item.image;
        }

        public ItemSO GetItemData()
        {
            return itemData;
        }
    }
}
