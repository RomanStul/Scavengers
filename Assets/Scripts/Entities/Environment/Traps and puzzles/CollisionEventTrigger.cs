using System;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Environment.Traps_and_puzzles
{
    public class CollisionEventTrigger : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private UnityEvent collisionEnterEvent, collisionExitEvent;
        [SerializeField] private UnityEvent triggerEnterEvent, triggerExitEvent;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private void OnCollisionEnter2D(Collision2D other)
        {
            collisionEnterEvent?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            triggerEnterEvent?.Invoke();
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            collisionExitEvent?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            triggerExitEvent?.Invoke();
        }
    }
}
