using System;
using Player.Module;
using ScriptableObjects.Material;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Environment.Traps_and_puzzles
{
    public class Grabber : MonoBehaviour
    {
        private static readonly int Target = Animator.StringToHash("Target");
        //================================================================CLASSES
        
        [Serializable]
        public class GrabberConstants
        {

        public float delayBeforeAttack, delayAfterAttack, damage, throwForce, maxRangeGrab;
        public MaterialSO.DamageType damageType;
        }
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private Animator animator;
        [SerializeField] private GrabberConstants grabberConstants;
        [SerializeField] private bool lookingForTarget = true;
        [SerializeField] private Transform mandibleCenter;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        private Transform target;
        private Transform snatchedObject;
        private Rigidbody2D snatchedRigidbody2D;
        
        public void OnTriggerStay2D(Collider2D other)
        {
            if (lookingForTarget)
            {
                target = other.transform;
                animator.SetTrigger(Target);
                lookingForTarget = false;
            }
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            Module m = other.transform.GetComponent<Module>();
            if (m)
            {
                HealthBar hb = m.GetScript<Player.Module.HealthBar>(Module.ScriptNames.HealthBarScript);
                float health = hb.TakeDamage(grabberConstants.damage, grabberConstants.damageType);
                // -up  because texture is oriented upside down
                if (health > 0)
                {
                    m.GetMoveRb().AddForce(-transform.up * grabberConstants.throwForce);
                }
            }
            else
            {
                if (other.gameObject.layer != LayerMask.NameToLayer("Player") && Vector2.Distance(Convertor.Vec3ToVec2(mandibleCenter.position), Convertor.Vec3ToVec2(other.transform.position)) < grabberConstants.maxRangeGrab)
                {
                    other.transform.position = mandibleCenter.position;
                    other.transform.parent = mandibleCenter;
                    Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Static;
                        snatchedRigidbody2D = rb;
                    }
                }
                else
                {
                    Debug.Log(other.gameObject.layer + " " + Vector3.Distance(mandibleCenter.position, other.transform.position));
                }
            }

        }

        public void AdjustRotation()
        {
            //inverted relative position because texture is upside down
            transform.rotation = Convertor.RotationConversion(transform.position - target.transform.position , transform);
        }

        public void ResetRotation()
        {
            transform.rotation = transform.parent.rotation;
        }

        public void SetLookingForTarget()
        {
            lookingForTarget = true;
            if (snatchedObject != null)
            {
                snatchedObject.parent = null;
                if (snatchedRigidbody2D != null)
                {
                    snatchedRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                }

                snatchedObject = null;
                snatchedRigidbody2D = null;
            }
        }
    }

}
