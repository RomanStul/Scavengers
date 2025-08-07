using System;
using Player.Module.Upgrades;
using ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("item")] public ItemCost[] neededItems;
        public ModuleUpgrades.Ups[] neededUpgrades;
        public string description;
        public ModuleUpgrades.Ups tag;
    }
}
