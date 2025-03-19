using UnityEngine;

namespace ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemSO", order = 1)]
    public class ItemSO : ScriptableObject
    {
        public enum Items
        {
            TestItem
        }
    
        public Sprite image;
        public Items itemType;
    }
}
