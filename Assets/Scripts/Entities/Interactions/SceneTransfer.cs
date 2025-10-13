using System.Collections;
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
        [SerializeField] private Vector3 position = Vector3.zero;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void LoadScene(Module module)
        {
            module.CreateStateObject();
            module.PrepareForSceneTransfer(position);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
