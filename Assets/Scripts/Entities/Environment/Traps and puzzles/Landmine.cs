using System;
using Player.Module;
using ScriptableObjects.Material;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Environment.Traps_and_puzzles
{
    public class Landmine : MonoBehaviour
    {
        //================================================================CLASSES

        [Serializable]
        public class LandmineParameters
        {
            public float damage;
            public float radius;
            public float force;
            public MaterialSO.DamageType damageType;
            public bool explodeOnTrigger = true;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private GameObject explodedVisual;
        [SerializeField] private GameObject unexplodedVisual;
        [SerializeField] private Collider2D col;
        [SerializeField] private LandmineParameters landmineParameters;
        [SerializeField] private UnityEvent onLandmineExploded;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void OnCollisionEnter2D(Collision2D collision)
        {
            onLandmineExploded.Invoke();
            if(landmineParameters.explodeOnTrigger) Explode(collision.gameObject);
        }

        public void Explode(GameObject trigger)
        {
            if (explodedVisual != null)
            {
                unexplodedVisual.SetActive(false);
                explodedVisual.SetActive(true);
                col.enabled = false;
            }
            else
            {
                gameObject.SetActive(false);
            }

            
            //For exploding by breaking
            if (trigger != null)
            {
                Module m = trigger.GetComponent<Module>();
                if (m != null)
                {
                    m.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript).TakeDamage(landmineParameters.damage, landmineParameters.damageType);
                    Vector2 forceDirection = (trigger.transform.position - transform.position) * 0.3f + 0.7f * transform.up;
                    forceDirection.Normalize();
                    m.GetMoveRb().AddForce( forceDirection * landmineParameters.force);
                }
            }
            
            //TODO check for health bars and RigidB's in radius
            
            
            
            
            
        }
    }
}
