using UnityEngine;

namespace Entities.Environment
{
    public class OreHolder : MonoBehaviour
    {
        public void SetOreIds(Transform parent = null)
        {
            if (parent == null)
            {
                parent = transform;
            }
            for (int i = 0; i < parent.childCount; i++)
            {
                Destructible id = parent.GetChild(i).GetComponent<Destructible>();
                if (id != null)
                {
                    id.SetId();
                }
                else
                {
                    if(parent.GetChild(i).childCount > 0)
                        SetOreIds(parent.GetChild(i));
                }
            }
        }
    }
}
