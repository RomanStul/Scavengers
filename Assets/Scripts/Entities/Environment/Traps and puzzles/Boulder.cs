using System;
using System.Collections;
using System.Collections.Generic;
using HelpScripts;
using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class Boulder : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private List<PulsePad> nearbyPads;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Movable movable;
        //================================================================GETTER SETTER
        
        public Rigidbody2D Rb { get { return rb; } set { rb = value; } }

        public bool SetToMove(PulsePad pad, Vector2 direction)
        {
            movable.SavePosition();
            if (isMoving)
            {
                return false;
            }
            isMoving = true;
            moveDirection = direction;
            lastUsedPad = pad;
            return true;
        }
        

        public bool IsMoving { get { return isMoving; } set{ isMoving = value; } }
        //================================================================FUNCTIONALITY
        private PulsePad lastUsedPad;
        private bool isMoving;
        private Vector2 moveDirection;
        private Boulder pushingBoulder;

        private void Awake()
        {
            Vector2 velocity = movable.SetSavedPosition();
            if (velocity != Vector2.zero)
            {
                RaycastHit2D hit = MyRaycast.RaycastCollider(transform.position, -velocity, Mathf.Infinity, ~LayerMask.GetMask("Player"));
                lastUsedPad = hit.collider.GetComponent<PulsePad>();
            }
            FindNearbyPads(false);            
        }

        private void TransferVelocity(Vector2 direction, RigidbodyConstraints2D constraints, Boulder originBoulder)
        {
            if (!isMoving)
            {
                Debug.Log(direction);
                SetToMove(null, direction);
                rb.constraints = constraints;
                Push();
                pushingBoulder = originBoulder;
            }

        }
        
        public void Push()
        {
            rb.AddForce(moveDirection * 150f, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionHandling(collision);
        }

        public void OnCollisionStay2D(Collision2D other)
        {
            CollisionHandling(other);
        }

        private void CollisionHandling(Collision2D collision)
        {

            if (isMoving)
            {

               
                if ((collision.gameObject.layer == LayerMask.NameToLayer("Player") && ShouldIgnorePlayerCollision(collision)))
                {
                    return;
                }
                
                if (collision.transform.GetComponent<Boulder>() != null)
                {
                    if (collision.transform.GetComponent<Boulder>() == pushingBoulder)
                    {
                        return;
                    }
                    
                    collision.transform.GetComponent<Boulder>().TransferVelocity(moveDirection, rb.constraints, this);
                }

                transform.position += 0.03f * Convertor.Vec2ToVec3(-moveDirection);
                isMoving = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            

                if ((collision.gameObject.layer == LayerMask.NameToLayer("Player") || !FindNearbyPads()))
                {
                    if(lastUsedPad != null)
                        StartCoroutine(lastUsedPad.RecallBoulder(this));
                    else
                        movable.SavePosition();
                }
            }
        }
        

        private bool ShouldIgnorePlayerCollision(Collision2D collision)
        {
            Vector3 offset = collision.transform.position - transform.position;
            float relevantOffsetAxis;
            float relevantMovementAxis;
            if (moveDirection.x > 0)
            {
                relevantOffsetAxis = offset.x;
                relevantMovementAxis = moveDirection.x;
            }
            else
            {
                relevantOffsetAxis = offset.y;
                relevantMovementAxis = moveDirection.y;
            }

            bool res = relevantMovementAxis * relevantOffsetAxis < 0;
            return relevantMovementAxis * relevantOffsetAxis < 0;
        }

        private bool FindNearbyPads(bool savePosition = true)
        {
            List<PulsePad> newPads = new List<PulsePad>();
            
            RaycastForPad(Vector2.up, newPads);
            RaycastForPad(Vector2.left, newPads);
            RaycastForPad(Vector2.down, newPads);
            RaycastForPad(Vector2.right, newPads);

            if (newPads.Count > 0)
            {
                Debug.Log(newPads.Count);
                foreach (PulsePad p in nearbyPads)
                {
                    p.ResetBoulder(this);
                }

                foreach (PulsePad p in newPads)
                {
                    p.SetBoulder(this);
                }
                nearbyPads = newPads;
                lastUsedPad = null;
                if(savePosition)
                    movable.SavePosition();
                pushingBoulder = null;

                return true;
            }

            return false;
        }

        private void RaycastForPad(Vector2 direction, List<PulsePad> pads)
        {
            RaycastHit2D hit = MyRaycast.RaycastCollider(Convertor.Vec3ToVec2(transform.position), direction, 1.3f);
            if (hit)
            {
                PulsePad pad = hit.transform.GetComponent<PulsePad>();
                if (pad != null)
                {
                    pads.Add(pad);
                }
            }
        }
    }
}
