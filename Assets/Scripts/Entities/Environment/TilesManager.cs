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
        [SerializeField] private Animator animator;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            animator.SetBool("show", false);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            animator.SetBool("show", true);
        }
        
    }
}
