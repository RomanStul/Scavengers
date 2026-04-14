using ScriptableObjects.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Environment.Traps_and_puzzles
{
    public class DoorController : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolSO targetedTool;
        [SerializeField] private SpriteRenderer keySpriteRenderer;

        [SerializeField] private UnityEvent correctToolEvent;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private bool interactedWithTool = false;

        public bool KeyLockOn()
        {
            if (interactedWithTool)
            {
                return false;
            }
            interactedWithTool = true;
            return true;
        }

        public void ClearKey()
        {
            interactedWithTool = false;
        }
        
        public bool IsCorrectTool(ToolSO tool)
        {
            if (tool == targetedTool)
            {
                correctToolEvent.Invoke();
                return true;
            }
            return false;
        }

        public void SetKeySprite(Sprite sprite)
        {
            keySpriteRenderer.sprite = sprite;
        }
    }
}
