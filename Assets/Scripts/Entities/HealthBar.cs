using System;
using System.Collections;
using Player.Module;
using ScriptableObjects.Material;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class HealthBar : BaseClass
    {
        //================================================================CLASSES
        [Serializable]
        public class HealthBarEvent : UnityEvent<Transform> { }
        
        [Serializable]
        public class HealthBarConstants
        {
            public float maxHealth, currentHealth;
            public bool maxAsStarting = true;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] protected HealthBarConstants healthBarConstants;
        [SerializeField] protected ScriptableObjects.Material.MaterialSO material;
        [SerializeField] protected HealthBarEvent onHealthChangedEvent;
        [SerializeField] protected HealthBarEvent onDestroyedEvent;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY


        private void Awake()
        {
            if (healthBarConstants.maxAsStarting)
            {
                healthBarConstants.currentHealth = healthBarConstants.maxHealth;
            }
        }

        public virtual float TakeDamage(float damage, MaterialSO.DamageType damageType)
        {
            float realDamage = ApplyDamageMultiplier(damage, damageType);
            if (realDamage <= 0)
            {
                return healthBarConstants.currentHealth;
            }
            
            healthBarConstants.currentHealth -= damage;
            if (healthBarConstants.currentHealth <= 0)
            {
                onDestroyedEvent?.Invoke(transform);
            }
            else
            {
                onHealthChangedEvent?.Invoke(transform);
            }
            return healthBarConstants.currentHealth;
        }

        protected virtual float ApplyDamageMultiplier(float damage, MaterialSO.DamageType damageType)
        {
            if (material == null || damageType == MaterialSO.DamageType.True)
            {
                return damage;
            }

            foreach (var multiplier in material.damageMultipliers)
            {
                if (multiplier.damageType == damageType)
                {
                    return damage * multiplier.damageMultiplier;
                }
            }

            return material.defaultDamage1 ? damage : 0f;
        }

    
        //============================================
        //HELPER FUNCTIONS

        public void DestroySelf(Transform target)
        {
            Destroy(target.gameObject);
        }
    }
}
