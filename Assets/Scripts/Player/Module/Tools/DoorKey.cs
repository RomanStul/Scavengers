using System;
using System.Collections;
using Entities;
using Entities.Environment.Traps_and_puzzles;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Player.Module.Tools
{
    public class DoorKey : ModuleTool
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private ToolSO keyTool;
        [SerializeField] private Sprite dropSprite;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        bool lockedOnDoorController = false;
        
        public override void Use(Module m)
        {
            StartCoroutine(TurnIntoDrop());
            rb.linearVelocity = Convertor.Vec3ToVec2((transform.position - m.transform.position).normalized) * 2;
        }

        private IEnumerator TurnIntoDrop()
        {
            yield return new WaitForSeconds(3f);
            if (!lockedOnDoorController)
            {
                SpawnDrop();
            }
        }

        private void SpawnDrop()
        {
            Item dropped = ItemDropper.SpawnItemAtRandomOffset(transform.position);
            dropped.SetToolData(keyTool);
            dropped.SetSprite(dropSprite);
            Destroy(gameObject);   
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DoorController dc = other.GetComponent<DoorController>();
            if (dc != null && !lockedOnDoorController && dc.KeyLockOn())
            {
                lockedOnDoorController = true;
                StartCoroutine(MoveToDoorController(dc));
            }
        }

        private IEnumerator MoveToDoorController(DoorController dc)
        {
            rb.bodyType = RigidbodyType2D.Static;
            float startTime = Time.time;
            Vector3 startPosition = transform.position;

            while (Time.time - startTime < 1f)
            {
                yield return null;
                transform.position = Vector3.Lerp(startPosition,dc.transform.position, Time.time - startTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, dc.transform.rotation, (Time.time - startTime) * 0.5f);
            }

            transform.position = dc.transform.position;
            
            
            yield return new WaitForSeconds(1f);

            bool isCorrectKey = dc.IsCorrectTool(keyTool);
            if (isCorrectKey)
            {
                Destroy(gameObject);
                dc.SetKeySprite(keyTool.icon);
            }
            else
            {
                SpawnDrop();
                dc.ClearKey();
            }
        }
    }
}
