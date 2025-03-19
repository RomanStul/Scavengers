using System;
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
        private float _armorDamageReduction = 0f;

        private void Awake()
        {
            if (ModuleRef.scripts.upgradesScript.IsActive(Upgrades.Upgrades.Ups.Armor))
            {
                _armorDamageReduction = collisionConstants.armorDamageReduction;
            }
        }
        
        public override void SetModule(Module module)
        {
            base.SetModule(module);
            MoveRigid = ModuleRef.GetMoveRb();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 relativePosition = Convertor.Vec3ToVec2(transform.position) - collision.contacts[0].point;
            Vector2 velocity = collision.relativeVelocity;
            float magnitude = Vector2.Dot(velocity, relativePosition);
            ModuleRef.scripts.healthBarScript.TakeDamage(magnitude * (collisionConstants.collisionDamageMultiplier - _armorDamageReduction));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject col = collision.gameObject;
            if (col.layer == LayerMask.NameToLayer("Item"))
            {
                ModuleRef.scripts.storageScript.AddItem(col.transform.GetComponent<Entities.Item>(), 1);
            }
        }
    }
}
