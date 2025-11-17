using Player.Module.Tools;
using UnityEngine;
using ScriptableObjects.Item;

namespace ScriptableObjects.Tools
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ToolSO", order = 3)]
    public class ToolSO : ScriptableObject
    {
        public enum ToolType
        {
            Acid_bomb,
            Repair_kit,
            Position_marker,
            Gravity_anchor
        }

        public Sprite icon;
        public ToolType toolType;
        public int price;
        public ItemSO[] neededItems;
        public ModuleTool executeObject;

    }
}
