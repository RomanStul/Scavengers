using Entities.Environment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.PortalWindow
{
    public class AnchorButton : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] Text text;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private DestructionManager.PermanentObject anchorInfo;
        private PortalWindow portalWindow;

        public void SetValues(DestructionManager.PermanentObject permanentObject, PortalWindow portalWindowRef)
        {
            anchorInfo = permanentObject;
            if (anchorInfo == null)
            {
                text.text = "Layer 0 portal";
            }
            else
            {
                text.text = permanentObject.Scene + " gravity anchor";
            }
            portalWindow = portalWindowRef;
        }

        public void Click()
        {
            portalWindow.ClickedAnchor(anchorInfo.Position, anchorInfo.Scene);
            DestructionManager.instance.RemovePermanentObject(anchorInfo.GameObject);
            Destroy(anchorInfo.GameObject);
        }
    }
}
