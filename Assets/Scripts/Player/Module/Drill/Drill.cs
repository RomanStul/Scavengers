using System;
using System.Collections;
using HelpScripts;
using ScriptableObjects.Material;
using UnityEngine;

namespace Player.Module.Drill
{
    public class Drill : MonoBehaviour
    {
        /*##########################################################
         * When activated drill starts animation that periodically
         * calls 'DamageTarget'.
         * Direction of lasers is changed in 'Update'.
         ##########################################################*/
        
        private static readonly int HitsPerSecond = Animator.StringToHash("HitsPerSecond");
        private static readonly int ChargeTime = Animator.StringToHash("ChargeTime");
        private static readonly int InUse = Animator.StringToHash("inUse");
        private static readonly int ExtensionMultiplier = Animator.StringToHash("ExtensionMultiplier");

        //================================================================CLASSES
        [Serializable]
        public class DrillConstants
        {
            public float range;
            public float chargeTime;
            public float hitsPerSecond;
            public int damage;
            public float extensionTime;
            public float drillOriginRotation = 1;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] protected DrillConstants drillConstants;
        [SerializeField] protected DrillController drillController;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Transform[] laserOrigins;

        public float laserExtended;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private Vector3 _targetPosition;
        private bool _currentlyUsing;
        private Transform _target;
        
        
        private void Awake()
        {
            animator.SetFloat(HitsPerSecond, drillConstants.hitsPerSecond);
            animator.SetFloat(ChargeTime, 1/drillConstants.chargeTime);
            animator.SetFloat(ExtensionMultiplier, 1/drillConstants.extensionTime);
        }

        private void Update()
        {
            if (_currentlyUsing)
            {
                FindTarget();
                ToggleLaser(true);
            }
        }

        public void Use(bool start)
        {
            _currentlyUsing = start;
            animator.SetBool(InUse, start);
            if(!start) ToggleLaser(false);
        }

        private void FindTarget()
        {
            _targetPosition = transform.position;

            RaycastHit2D hit = MyRaycast.RaycastCollider(Convertor.Vec3ToVec2(transform.position), Convertor.Vec3ToVec2(transform.up), drillConstants.range);
            if (hit)
            {
                _targetPosition = hit.point;
                _target = hit.transform;
            }
            else
            {
                _targetPosition = transform.position + transform.up * drillConstants.range;
                _target = null;
            }
        }

        private void ToggleLaser(bool toggleState)
        {
            if (!toggleState)
            {
                foreach (var origin in laserOrigins)
                {
                    Vector3 scale = origin.localScale;
                    origin.localScale = new Vector3(scale.x, 0, scale.z);
                }
                return;
            }
            
            //TODO change length of lasers during lerp not before
            foreach (var origin in laserOrigins)
            {
                Vector3 targetPositionMod = _targetPosition - origin.position;
                Convertor.Lerp2D(targetPositionMod, origin, drillConstants.drillOriginRotation);
                
                
                float distance = DistanceToIntersectionPoint(Convertor.Vec3ToVec2(origin.position), Convertor.Vec3ToVec2(origin.up),Convertor.Vec3ToVec2(transform.position), Convertor.Vec3ToVec2(transform.up));
                Vector3 scale = origin.localScale;
                origin.localScale = new Vector3(scale.x, distance * laserExtended, 0);
            }
        }

        //Called from drill animation 'Using'
        public void DamageTarget()
        {
            if (_target == null) return;
            
            Entities.HealthBar healthBar = _target.GetComponent<Entities.HealthBar>();
            if (healthBar != null)
            {
                healthBar.TakeDamage(drillConstants.damage, MaterialSO.DamageType.Plasma);
            }
        }

        private float DistanceToIntersectionPoint(Vector2 position1, Vector2 forward1, Vector2 position2, Vector2 forward2)
        {
            Vector2 deltaPosition = position2 - position1;
            float denominator = forward1.x * forward2.y - forward1.y * forward2.x;

            if (denominator == 0) return 0;

            float t1 = (deltaPosition.x * forward2.y - deltaPosition.y * forward2.x) / denominator;

            Vector2 intersectionPoint = position1 + t1 * forward1;
            
            return Vector2.Distance(intersectionPoint, position1);
        }
    }
}
