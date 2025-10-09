using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Entities.Environment
{
    public class TilesManager : MonoBehaviour
    {
        //================================================================CLASSES
    
        //================================================================EDITOR VARIABLES

        [SerializeField] private TilemapRenderer tilemapRenderer;
        [SerializeField] private Tilemap tilemap;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private float oppacity = 1f;
        
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            StartCoroutine(ChangeVisibility(false));
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            StartCoroutine(ChangeVisibility(true));
        }

        private IEnumerator ChangeVisibility(bool shouldBeVisible)
        {
            while (!Mathf.Approximately(oppacity, (shouldBeVisible ? 1f : 0f)))
            {
                oppacity += shouldBeVisible ? Time.deltaTime*8 : -Time.deltaTime*8;
                oppacity = Mathf.Clamp(oppacity, 0f, 1f);
                tilemap.color = new Color(1f, 1f, 1f, oppacity);
                yield return null;
            }

            
        }
    }
}
