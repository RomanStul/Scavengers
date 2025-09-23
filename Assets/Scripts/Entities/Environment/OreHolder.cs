using UnityEngine;

namespace Entities.Environment
{
    public class OreHolder : MonoBehaviour
    {
        public void SetOreIds()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destructible id = transform.GetChild(i).GetComponent<Destructible>();
                if (id != null)
                {
                    id.SetId();
                }
            }
        }
    }
}
