using UnityEngine;

namespace Entities.Environment
{
    public class Environment : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] public float fuelConsumptionMultiplier = 1.0f;
        [SerializeField] public float damageMultiplier = 1.0f;

        [SerializeField] public GameObject module;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        public static Environment instance;

        void Awake()
        {
            instance = this;
            if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
            {
                Instantiate(module);
            }
        }
        
    }
}
