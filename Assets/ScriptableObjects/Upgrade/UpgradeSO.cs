using Player.Module.Upgrades;
using ScriptableObjects.Item;
using UnityEngine;

namespace ScriptableObjects.Upgrade
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/UpgradeSO", order = 2)]
    public class UpgradeSO : ScriptableObject
    {
        public int cost;
        public ItemSO[] item;
        public Upgrades.Ups[] neededUpgrades;
        public string description;
        public Upgrades.Ups tag;
    }
}
