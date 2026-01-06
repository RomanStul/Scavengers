using System.Collections;
using UnityEngine;

namespace Player.UI
{
    public class HelpDisplay : UIWindow
    {
        //================================================================CLASSES
        
        public enum DisplayModes
        {
            OnlyMovement,
            OnlyDrill,
            All
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private GameObject movementHelp, drillHelp, otherHelp;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void SetMode(DisplayModes mode)
        {
            switch (mode)
            {
                case DisplayModes.OnlyMovement:
                    movementHelp.SetActive(true);
                    drillHelp.SetActive(false);
                    otherHelp.SetActive(false);
                    break;
                case DisplayModes.OnlyDrill:
                    movementHelp.SetActive(false);
                    drillHelp.SetActive(true);
                    otherHelp.SetActive(false);
                    break;
                case DisplayModes.All:
                    movementHelp.SetActive(true);
                    drillHelp.SetActive(true);
                    otherHelp.SetActive(true);
                    break;
            }
        }
    }
}
