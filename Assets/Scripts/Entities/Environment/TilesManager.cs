using UnityEngine;
using UnityEngine.Tilemaps;

namespace Entities.Environment
{
    public class TilesManager : MonoBehaviour
    {
        //================================================================CLASSES
    
        //================================================================EDITOR VARIABLES

        [SerializeField] private TilemapRenderer tilemapRenderer;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void OnTriggerEnter2D(Collider2D other)
        {
            tilemapRenderer.enabled = false;
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            tilemapRenderer.enabled = true;
        }
    }
}
