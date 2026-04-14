using System;
using System.Collections;
using Entities;
using Entities.Interactions;
using Player.Module.Upgrades;
using ScriptableObjects.Material;
using UnityEditor;
using UnityEngine;

namespace Player.Module
{
    public class CollisionHandler : ModuleBaseScript
    {
        //================================================================CLASSES
        [Serializable]
        public class CollisionConstants
        {
            public float armorDamageReduction;
            public float collisionDamageMultiplier;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private CollisionConstants collisionConstants;
        //================================================================GETTER SETTER
        public void IncreaseAcid(float value, bool increase)
        {
            if (increase)
            {
                if (Mathf.Floor(acidCoverage) < Mathf.Floor(acidCoverage + value))
                {
                    acidCoverage = ModuleRef.GetScript<UpgradeVisuals>(Module.ScriptNames.UpgradeVisualsScript).ChangeAcidTexture(acidCoverage + value);
                }
                else
                {
                    acidCoverage += value;
                }
            }
            else
            {
                if (acidCoverage-value >= 0 && Mathf.Floor(acidCoverage) > Mathf.Floor(acidCoverage - value))
                {
                    acidCoverage = ModuleRef.GetScript<UpgradeVisuals>(Module.ScriptNames.UpgradeVisualsScript).ChangeAcidTexture(acidCoverage - value);
                }
                else
                {
                    acidCoverage -= value;
                }
                if(acidCoverage < 0) acidCoverage = 0;
            }

            
        }
        //================================================================FUNCTIONALITY
        private float acidCoverage = 0;
        private bool inAcid = false;

        protected Rigidbody2D MoveRigid;
        
        public override void SetModule(Module module)
        {
            base.SetModule(module);
            MoveRigid = ModuleRef.GetMoveRb();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
            {
                return;
            }
            //TODO add point to calculate collision magnitude from, take one that's closer to point of impact
            Vector2 relativePosition = (Convertor.Vec3ToVec2(transform.position) - collision.GetContact(0).point).normalized;
            Vector2 velocity = collision.relativeVelocity;
            float magnitude = Mathf.Abs(Vector2.Dot(velocity, relativePosition));
            ModuleRef.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript).TakeDamage(magnitude * (collisionConstants.collisionDamageMultiplier), MaterialSO.DamageType.Kinetic);

            Entities.HealthBar bar = collision.gameObject.transform.GetComponent<Entities.HealthBar>();
            if(bar != null) bar.TakeDamage(magnitude, MaterialSO.DamageType.Kinetic);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject col = collision.gameObject;

            if (col.layer == LayerMask.NameToLayer("Item"))
            {
                ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).PickUpItem(col.transform.GetComponent<Entities.Item>(), 1);
                return;
            }

            if (col.layer == LayerMask.NameToLayer("Interactable"))
            {
                ModuleRef.GetScript<InteractionHandler>(Module.ScriptNames.InteractionScript).SetInteractableEntity(col.transform.GetComponent<Interactable>());
            }

            if (col.layer == LayerMask.NameToLayer("Acid"))
            {
                ModuleRef.GetMoveRb().linearDamping *= 80;
                ModuleRef.GetMoveRb().angularDamping *= 5;
                ModuleRef.GetScript<UpgradeVisuals>(Module.ScriptNames.UpgradeVisualsScript).ShowAcidTexture();
                inAcid = true;
            }
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameObject col = collision.gameObject;

            if (col.layer == LayerMask.NameToLayer("Interactable"))
            {
                ModuleRef.GetScript<InteractionHandler>(Module.ScriptNames.InteractionScript).ResetInteractableEntity(col.transform.GetComponent<Interactable>());
            }
            
            if (col.layer == LayerMask.NameToLayer("Acid"))
            {
                ModuleRef.GetMoveRb().linearDamping /= 80;
                ModuleRef.GetMoveRb().angularDamping /= 5;
                inAcid = false;
            }
        }

        private void Update()
        { 
            IncreaseAcid(Time.deltaTime, inAcid);
        }
    }
}
