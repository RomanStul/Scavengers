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

        public float delayBeforeAttack, delayAfterAttack, damage, throwForce;
        public MaterialSO.DamageType damageType;
        }
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private Animator animator;
        [SerializeField] private GrabberConstants grabberConstants;
        [SerializeField] private bool lookingForTarget = true;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        private Transform target;
        
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
            if (m == null)
            {
                return;
            }
            HealthBar hb = m.GetScript<Player.Module.HealthBar>(Module.ScriptNames.HealthBarScript);
            hb.TakeDamage(grabberConstants.damage, grabberConstants.damageType);
            // -up  because texture is oriented upside down
            m.GetMoveRb().AddForce(-transform.up * grabberConstants.throwForce);
        }

        public void AdjustRotation()
        {
            //inverted relative position because texture is upside down
            transform.rotation = Convertor.RotationConversion(transform.position - target.transform.position , transform);
        }

        public void ResetRotation()
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }

        public void SetLookingForTarget()
        {
            lookingForTarget = true;
        }
    }

}
