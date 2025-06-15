using UnityEngine;
using Player.Module;
using UnityEngine.SceneManagement;

namespace Entities.Interactions
{
    public class SceneTransfer : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private string sceneToLoad;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void LoadScene(Module module)
        {
            module.CreateStateObject();
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
