using UnityEngine;

namespace Player.Module.Drill
{
    public class Drill : MonoBehaviour
    {
        //================================================================
        //================================================================
        //================================================================
    
        public void Use(bool start)
        {
            transform.GetComponent<SpriteRenderer>().color = start ? Color.yellow: Color.red;
        }
    }
}
