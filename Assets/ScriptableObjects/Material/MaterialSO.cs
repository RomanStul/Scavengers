using System;
using UnityEngine;

namespace ScriptableObjects.Material
{
    [CreateAssetMenu(fileName = "Material", menuName = "ScriptableObjects/MaterialSO", order = 1)]
    public class MaterialSO : ScriptableObject
    {
        public enum DamageType
        {
            Kinetic,
            Electric,
            Plasma,
            True
        }

        [Serializable]
        public class DamageMultiplier
        {
            public DamageType damageType;
            public float damageMultiplier;
        }
        
        public bool defaultDamage1 = true;
        public DamageMultiplier[] damageMultipliers;
    }
}
