using System;
using ScriptableObjects.Item;
using UnityEngine;

namespace ScriptableObjects.Day
{
    [Serializable]
    public class OrePriceChanges
    {
        public ItemSO.Items item;
        public float multiplier;
    }
    
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/DaySO", order = 1)]
    public class DaySO : ScriptableObject
    {
        public int dayNumber;
        public float refuelCostMultiplier = -1;
        public float repairCostMultiplier = -1;
        public int evacuateCost = -1;
        public int startOfDayPayment = -1;
        public OrePriceChanges[] priceChanges;

    }

 
}
