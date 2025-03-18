using System;
using System.Collections;
using Player.Module;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class HealthBar : MonoBehaviour
    {
        //================================================================CLASSES
        [Serializable]
        public class HealthBarEvent : UnityEvent<Transform> { }
        
        [Serializable]
        public class HealthBarConstants
        {
            public int maxHealth, currentHealth;
            public bool maxAsStarting = true;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] protected HealthBarConstants healthBarConstants;
        //TODO add material SO
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

        public int TakeDamage(int damage)
        {
            //TODO add material damage multiplier
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

    
        //============================================
        //HELPER FUNCTIONS

        public void DestroySelf(Transform target)
        {
            Destroy(target.gameObject);
        }
    }
}
