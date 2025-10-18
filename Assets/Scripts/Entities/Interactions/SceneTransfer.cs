using System.Collections;
using UnityEngine;
using Player.Module;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Entities.Interactions
{
    public class SceneTransfer : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private string sceneToLoad;
        [FormerlySerializedAs("position")] [SerializeField] private Vector2 positionToSpawn = Vector2.zero;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void LoadScene(Module module)
        {
            module.CreateStateObject(sceneToLoad, positionToSpawn);
            module.PrepareForSceneTransfer(positionToSpawn);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
