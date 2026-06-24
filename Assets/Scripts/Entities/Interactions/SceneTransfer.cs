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
        [SerializeField] protected string sceneToLoad;
        [FormerlySerializedAs("position")] [SerializeField] protected Vector2 positionToSpawn = Vector2.zero;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void LoadScene(Module module)
        {
            LoadScene(module, positionToSpawn, sceneToLoad);
        }

        protected void LoadScene(Module module, Vector2 position, string scene)
        {
            module.CreateStateObject(scene, position);
            module.PrepareForSceneTransfer(position, scene, transform.position);
        }
    }
}
