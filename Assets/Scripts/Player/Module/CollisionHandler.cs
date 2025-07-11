using System;
using System.Collections;
using Entities;
using Entities.Interactions;
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
        //================================================================FUNCTIONALITY

        protected Rigidbody2D MoveRigid;
        
        public override void SetModule(Module module)
        {
            base.SetModule(module);
            MoveRigid = ModuleRef.GetMoveRb();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //TODO add point to calculate collision magnitude from, take one that's closer to point of impact
            Vector2 relativePosition = Convertor.Vec3ToVec2(transform.position) - collision.contacts[0].point;
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

            if (col.layer == LayerMask.NameToLayer("Interactible"))
            {
                ModuleRef.GetScript<InteractionHandler>(Module.ScriptNames.InteractionScript).SetInteractableEntity(col.transform.GetComponent<Interactable>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameObject col = collision.gameObject;

            if (col.layer == LayerMask.NameToLayer("Interactible"))
            {
                ModuleRef.GetScript<InteractionHandler>(Module.ScriptNames.InteractionScript).ResetInteractableEntity(col.transform.GetComponent<Interactable>());
            }
        }
    }
}
