using Entities.Environment;
using Entities.Interactions;
using UnityEngine;

namespace Player.UI.PortalWindow
{
    public class PortalWindow : UIWindow
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private AnchorButton AnchorButtonPrefab;
        [SerializeField] private GameObject AnchorButtonContainer;
        //================================================================GETTER SETTER

        public void SetPortalRef(OutpostPortal portalRef)
        {
            portal = portalRef;
        }
        //================================================================FUNCTIONALITY
        private OutpostPortal portal;
        
        
        public override bool ToggleWindow()
        {
            bool ret = base.ToggleWindow();
            if (ret)
            {
                SetUpWindow();
            }
            else
            {
                ResetWindow();
            }
            return ret;
        }

        private void SetUpWindow()
        {
            AnchorButton portalButton = Instantiate(AnchorButtonPrefab, AnchorButtonContainer.transform);
            portalButton.SetValues(null, this);
            
            foreach (DestructionManager.PermanentObject permObject in DestructionManager.instance.GetPermanentObjects())
            {
                if (permObject.Type == DestructionManager.PermanentObject.permanentObjectType.GravityAnchor)
                {
                    AnchorButton anchorButton = Instantiate(AnchorButtonPrefab, AnchorButtonContainer.transform);
                    anchorButton.SetValues(permObject, this);
                }
            }    
        }

        private void ResetWindow()
        {
            for (int i = 0; i < AnchorButtonContainer.transform.childCount; i++)
            {
                Destroy(AnchorButtonContainer.transform.GetChild(i).gameObject);
            }
        }

        public void ClickedAnchor(Vector2 position, string scene)
        {
            portal.Transfer(scene, position);
        }
        
    }
}
