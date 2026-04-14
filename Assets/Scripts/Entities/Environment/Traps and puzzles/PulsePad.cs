using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Entities.Environment.Traps_and_puzzles
{
    public class PulsePad : PulseOre
    { 
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private Boulder boulder;
        //================================================================GETTER SETTER

        public void SetBoulder(Boulder b)
        {
            boulder = b;
            b.transform.position = Mathf.Abs(transform.up.x) > 0 ? new Vector3(b.transform.position.x, transform.position.y, -1) : new Vector3(transform.position.x, b.transform.position.y, -1);
        }

        public void ResetBoulder(Boulder b)
        {
            if (boulder == b)
            {
                boulder = null;
            }
        }
        
        //================================================================FUNCTIONALITY
        


        public override void MakePulse()
        {
            if (boulder != null && boulder.SetToMove(this, transform.up))
            {
                boulder.Rb.constraints = (Mathf.Abs(transform.up.x) > 0.5f ? RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.FreezePositionX);
                boulder.Rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
                
                boulder.Push();
            }
            healthBar.HealHealth(-1);
            particles.Play();
            ChangeVisuals();
        }

        public IEnumerator RecallBoulder(Boulder b)
        {
            yield return new WaitForSeconds(4f);

            b.Rb.constraints = (Mathf.Abs(transform.up.x) > 0.5f ? RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.FreezePositionX);
            b.Rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
            b.SetToMove(this, -transform.up);
            b.IsMoving = true;
            while (b.Rb.linearVelocity.magnitude < 1f && b.IsMoving)
            {
                b.Rb.AddForce(-transform.up * (2500 * Time.deltaTime));
                yield return null;
            }
            
            Debug.Log("recall ended");
                
        }
        
        
    }
}
