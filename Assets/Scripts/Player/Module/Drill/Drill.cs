using System;
using System.Collections;
using UnityEngine;

namespace Player.Module.Drill
{
    public class Drill : MonoBehaviour
    {
        private static readonly int HitsPerSecond = Animator.StringToHash("HitsPerSecond");
        private static readonly int ChargeTime = Animator.StringToHash("ChargeTime");
        private static readonly int InUse = Animator.StringToHash("inUse");
        private static readonly int ExtensionMultiplier = Animator.StringToHash("ExtensionMultiplier");

        //================================================================
        [SerializeField] protected Player.Module.Module.DrillConstants drillConstants;
        [SerializeField] protected DrillController drillController;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Transform[] laserOrigins;

        public float laserExtended;
        //================================================================
        //================================================================
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

        public void FindTarget()
        {
            _targetPosition = transform.position;
            
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, drillConstants.range))
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
                    origin.localScale = new Vector3(scale.x, scale.y, 0);
                }
                return;
            }
            
            //TODO lerp the rotation of lasers instead of lookAT
            foreach (var origin in laserOrigins)
            {
                origin.LookAt(_targetPosition, Vector3.forward);
                float distance = Vector3.Distance(_targetPosition, origin.position);
                Vector3 scale = origin.localScale;
                origin.localScale = new Vector3(scale.x, scale.y, distance * laserExtended);
            }
        }

        public void DamageTarget()
        {
            if (_target == null) return;

            Debug.Log("Damaging target " + _target.gameObject.name);
        }
    }
}
