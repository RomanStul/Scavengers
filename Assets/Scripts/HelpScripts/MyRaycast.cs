using UnityEngine;

namespace HelpScripts
{
    public class MyRaycast : MonoBehaviour
    {
        public static RaycastHit2D RaycastTrigger(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers)
        {
            return Physics2D.Raycast(origin, direction, distance, layerMask, -1.5f, -0.5f);
        }

        public static RaycastHit2D RaycastCollider(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers)
        {
            return Physics2D.Raycast(origin, direction, distance, layerMask, -0.5f);
        }

        public static RaycastHit2D RaycastBoth(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers)
        {
            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }
    }
}
