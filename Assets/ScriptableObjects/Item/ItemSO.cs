using UnityEngine;

namespace ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemSO", order = 1)]
    public class ItemSO : ScriptableObject
    {
        public enum Items
        {
            Test_Item,
            Titanium_Ore,
            Sulfur_Ore
        }
    
        public Sprite image;
        public Items itemType;
        public int price;
    }
}
