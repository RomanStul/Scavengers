using UnityEngine;

namespace ScriptableObjects.Day
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/DaySO", order = 1)]
    public class DaySO : ScriptableObject
    {
        public int dayNumber;
        public float refuelCostMultiplier = -1;
        public float repairCostMultiplier = -1;
        public int startOfDayPayment = -1;

    }
}
