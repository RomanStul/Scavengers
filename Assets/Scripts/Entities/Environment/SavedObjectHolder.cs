using UnityEngine;

namespace Entities.Environment
{
    public class SavedObjectHolder : MonoBehaviour
    {
        public void SetId(Transform parent = null)
        {
            if (parent == null)
            {
                parent = transform;
            }
            for (int i = 0; i < parent.childCount; i++)
            {
                SaveIDDistibutor id = parent.GetChild(i).GetComponent<SaveIDDistibutor>();
                if (id != null)
                {
                    id.SetId();
                }
                else
                {
                    if(parent.GetChild(i).childCount > 0)
                        SetId(parent.GetChild(i));
                }
            }
        }
    }
}
