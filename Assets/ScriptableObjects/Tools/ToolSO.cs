using UnityEngine;
using ScriptableObjects.Item;

namespace ScriptableObjects.Tools
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ToolSO", order = 3)]
    public class ToolSO : ScriptableObject
    {
        public enum ToolType
        {
            Acid_bomb
        }

        public Sprite icon;
        public ToolType toolType;
        public int price;
        public ItemSO[] neededItems;
        
    }
}
