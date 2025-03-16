using System;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class HealthBar : MonoBehaviour
    {
        [Serializable]

        public class HealthBarConstants
        {
            public int maxHealth, currentHealth;
            public bool maxAsStarting;
        }
        //================================================================
        [SerializeField] protected HealthBarConstants healthBarConstants;
        //TODO add material SO
        [SerializeField] protected UnityEvent onDestroyed;
        //================================================================
        //================================================================

        private void Awake()
        {
            if (healthBarConstants.maxAsStarting)
            {
                healthBarConstants.currentHealth = healthBarConstants.maxHealth;
            }
        }

        public int takeDamage(int damage)
        {
            //TODO add material damage multiplier
            healthBarConstants.currentHealth -= damage;
            if (healthBarConstants.currentHealth <= 0)
            {
                onDestroyed?.Invoke();
            }
            return healthBarConstants.currentHealth;
        }

    
    
    }
}
