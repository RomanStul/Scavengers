using Player.Module;
using UnityEngine;

namespace Entities.Environment
{
    public class Environment : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] public float fuelConsumptionMultiplier = 1.0f;
        [SerializeField] public float damageMultiplier = 1.0f;
        [SerializeField] public float timerMultiplier = 1.0f;
        [SerializeField] private Vector2 moduleSpawnLocation = new Vector2(0,0);
        [SerializeField] private Sprite minimap;
        [SerializeField] private Vector2Int minimapCenter;

        [SerializeField] public GameObject module;
        //================================================================GETTER SETTER
        public Vector2 GetModuleSpawnLocation()
        {
            return moduleSpawnLocation;
        }

        public Vector2 GetMinimapCenter()
        {
            return minimapCenter;
        }

        public Sprite GetMinimap()
        {
            return minimap;
        }

        public Module GetModuleRef()
        {
            return moduleRef;
        }
        //================================================================FUNCTIONALITY
        public static Environment instance;
        private Module moduleRef;
        
        void Awake()
        {
            instance = this;
            if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
            {
                moduleRef = Instantiate(module, new Vector3(moduleSpawnLocation.x, moduleSpawnLocation.y, 0), Quaternion.identity).GetComponent<Module>();
                
            }
            else
            {
                moduleRef = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Module>();
            }
        }
        
    }
}
