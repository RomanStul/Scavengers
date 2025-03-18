using System;
using UnityEngine;

namespace Player.Module
{
    public class CollisionHandler : ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
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
            Debug.Log("collision " + collision.relativeVelocity);
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
