using Player.Module;
using UnityEngine;

namespace Entities
{
    public class Interactable : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private GameObject[] activeVisuals;
        [SerializeField] private GameObject useVisualCue;
        [SerializeField] private InteractionHandler.InteractionType interactionType;
        [SerializeField] private bool usedRepeatably;
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private Module moduleRef;
        private bool used = false;

        public void Activate(bool active, Module module)
        {
            if (used && !usedRepeatably && active)
            {
                return;
            }
            moduleRef = module;
            for(int i = 0; i < activeVisuals.Length; i++)
            {
                activeVisuals[i].SetActive(!activeVisuals[i].activeSelf);
            }
            useVisualCue.SetActive(active);
        }

        public void Use()
        {
            if (used && !usedRepeatably)
            {
                return;
            }
            
            if (!usedRepeatably)
            {
                useVisualCue.SetActive(false);
            }
            used = true;
            //TODO play event or something pass moduleRef into event

            if (!usedRepeatably)
            {
                Activate(false, moduleRef);
            }
        }
    }
}
