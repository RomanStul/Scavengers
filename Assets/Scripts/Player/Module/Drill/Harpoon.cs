using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player.Module.Drill
{
    public class Harpoon : MonoBehaviour
    {
        //================================================================CLASSES
        [Serializable]

        public class HarpoonConstants
        {
            public float range;
            public float primaryDetectionCircleRadius;
            public float secondaryDetectionCircleRadius;
            public float launchForce;
            public float recallSpeed;
            public float maxAttachDistance;
            public Color normalColor, breakColor;
        }


        public enum HarpoonStatus
        {
            Ready,
            Launched,
            Recalling,
            Attached
        }
        //================================================================EDITOR VARIABLES

        [SerializeField] private BoxCollider2D trigger;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private HarpoonConstants harpoonConstants;
        [SerializeField] private HarpoonStatus status = HarpoonStatus.Ready;

        [SerializeField] private SpringJoint2D joint;

        [SerializeField] private LineRenderer lineRend;
        //================================================================GETTER SETTER
        public void SetModuleRb(Rigidbody2D mrb)
        {
            moduleRb = mrb;
        }
        //================================================================FUNCTIONALITY
        private Vector3 launchPoint;
        private Item collectedItem = null;
        private Drill drillRef;
        private bool shouldAttach = true;
        private Vector3 lastDrillPosition = Vector3.zero;
        private Rigidbody2D moduleRb;
        
        public void UseHarpoon(Drill drill)
        {
            if(status != HarpoonStatus.Ready) return;

            drillRef = drill;
            Item target = LookForTarget();
            
            LaunchHarpoon(target, drill);
        }
        
        private Item LookForTarget()
        {
            Item target = null;
            target = CircleDetection(Convertor.Vec3ToVec2(transform.position + transform.up * harpoonConstants.primaryDetectionCircleRadius), harpoonConstants.primaryDetectionCircleRadius);
            if (target == null || target.GetIsCollecting())
            {
                Item secondTarget = CircleDetection(Convertor.Vec3ToVec2(transform.position + transform.up * (harpoonConstants.primaryDetectionCircleRadius * 2 + harpoonConstants.secondaryDetectionCircleRadius) ), harpoonConstants.secondaryDetectionCircleRadius);
                if (target == null ||
                    target.GetIsCollecting() && secondTarget != null && !secondTarget.GetIsCollecting())
                {
                    target = secondTarget;
                }
            }

            return target;
        }

        private Item CircleDetection(Vector2 origin, float radius)
        {
            List<Collider2D> results = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();

            filter.NoFilter();

            Physics2D.OverlapCircle(origin,radius, filter, results);


            Item bestMatch = null;
            bool isCurrentlyCollecting = true;
            foreach (Collider2D coll in results)
            {
                if (coll.gameObject.layer == LayerMask.NameToLayer("Item"))
                {
                    Item detectedItem = coll.gameObject.GetComponent<Item>();
                    if (isCurrentlyCollecting == false || bestMatch == null)
                    {
                        bestMatch = detectedItem;
                        isCurrentlyCollecting = detectedItem != null && detectedItem.GetIsCollecting();
                    }
                }
            }
            
            return bestMatch;
        }
        
        private void LaunchHarpoon(Item target, Drill drill)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            trigger.enabled = true;
            status = HarpoonStatus.Launched;
            launchPoint = transform.position;
            shouldAttach = true;
            transform.parent = null;
            rb.linearVelocity = 0.5f * moduleRb.linearVelocity;

            Vector2 launchDirection;
            
            if (target == null)
            {
                launchDirection = drill.transform.up;
            }
            else
            {
                launchDirection = Convertor.Vec3ToVec2(target.transform.position - transform.position);
            }
            
            rb.AddForce(launchDirection.normalized * harpoonConstants.launchForce);
        }

        private void FixedUpdate()
        {
            RenderLine();

            if (status == HarpoonStatus.Ready)
            {
                return;
            }

            
            if (status == HarpoonStatus.Recalling)
            {
                Vector3 drillMove = drillRef.transform.position - lastDrillPosition;
    
                Vector3 direction = (drillRef.transform.position - transform.position).normalized;
                Vector3 recallMove = direction * (harpoonConstants.recallSpeed * Time.fixedDeltaTime);


                transform.position += drillMove + recallMove;
                
                lastDrillPosition = drillRef.transform.position;
                
                if (Vector3.Distance(transform.position, drillRef.transform.position) < 0.1f)
                {
                    status = HarpoonStatus.Ready;
                    
                    transform.position = drillRef.transform.position;
                    transform.rotation = drillRef.transform.rotation;
                    transform.parent = drillRef.transform;
                    rb.simulated = false;
                    
                    
                    drillRef.HarpoonIsRecalled(collectedItem);
                    if (collectedItem != null)
                    {
                        collectedItem.transform.parent = null;
                        SceneManager.MoveGameObjectToScene(collectedItem.gameObject, SceneManager.GetActiveScene());
                        collectedItem.TurnOnTriggers();
                    }
                    collectedItem = null;
                }
            }
            
            if (status == HarpoonStatus.Launched && Vector3.Distance(transform.position, launchPoint) > harpoonConstants.range)
            {
                StartRecalling();
            }
            
            
        }

        private void RenderLine()
        {
            if (status == HarpoonStatus.Attached)
            {
                float magnitude = joint.reactionForce.magnitude;
                float maxForce = joint.breakForce;

                lineRend.endColor = Color.Lerp(harpoonConstants.normalColor, harpoonConstants.breakColor,
                    magnitude / maxForce);
                lineRend.startColor = lineRend.endColor;

                lineRend.enabled = true;
                lineRend.SetPosition(0, joint.transform.position);
                lineRend.SetPosition(1, transform.position);
            }
            else
            {
                lineRend.enabled = false;
            }
        }

        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            StartRecalling();
        }

        private void StartRecalling()
        {
            lastDrillPosition = drillRef.transform.position;
            status = HarpoonStatus.Recalling;
            trigger.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        public void Detach()
        {
            if (status == HarpoonStatus.Launched)
            {
                shouldAttach = false;
                return;
            }

            if (status == HarpoonStatus.Attached)
            {
                joint.enabled = false;
                StartRecalling();
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                collectedItem = collision.gameObject.GetComponent<Item>();
                collectedItem.transform.parent = transform;
                collectedItem.transform.localPosition = Vector3.zero;
                collectedItem.TurnOffTriggers();
                StartRecalling();
            }
            else
            {
                if(collision.isTrigger || !shouldAttach || Vector3.Distance(transform.position, drillRef.transform.position) > harpoonConstants.maxAttachDistance) return;
                
                status = HarpoonStatus.Attached;
                joint.connectedAnchor = Convertor.Vec3ToVec2(transform.position);
                joint.enabled = true;
                rb.bodyType = RigidbodyType2D.Static;
                trigger.enabled = false;
            }
            
            
        }
    }
}
