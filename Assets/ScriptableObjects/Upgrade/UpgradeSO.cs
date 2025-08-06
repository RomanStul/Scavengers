using System;
using Player.Module.Upgrades;
using ScriptableObjects.Item;
using UnityEngine;

namespace ScriptableObjects.Upgrade
{
    
    
    [Serializable]
    public class ItemCost
    {
        public ItemSO item;
        public int amount;
    } 

    [CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/UpgradeSO", order = 2)]
    
    public class UpgradeSO : ScriptableObject
    {
        public int cost;
        public ItemCost[] item;
        public ModuleUpgrades.Ups[] neededUpgrades;
        public string description;
        public ModuleUpgrades.Ups tag;
    }
}
