using Player.Module.Tools;
using Player.Module.Upgrades;
using UnityEngine;
using ScriptableObjects.Item;
using ScriptableObjects.Upgrade;

namespace ScriptableObjects.Tools
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ToolSO", order = 3)]
    public class ToolSO : ScriptableObject
    {
        public enum ToolType
        {
            Explosive,
            Acid_bomb,
            Repair_kit,
            Position_marker,
            Gravity_anchor
        }

        public string description;
        public Sprite icon;
        public ToolType toolType;
        public int price;
        public ModuleUpgrades.Ups unlockUpgrade;
        public ItemCost[] neededItems;
        public ModuleTool executeObject;
        

    }
}
