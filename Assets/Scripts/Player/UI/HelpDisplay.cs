using System;
using System.Collections;
using Player.Module.Upgrades;
using UnityEngine;

namespace Player.UI
{
    public class HelpDisplay : UIWindow
    {
        //================================================================CLASSES
        
        public enum DisplayModes
        {
            All,
            OnlyMovement,
            OnlyDrill,
            OnlyTools,
            OnlySide,
            OnlyDash,
            OnlyStop,
            Inventory,

        }
        [Serializable]
        public class DisplayModeComponent
        {
            public GameObject helpObject;
            public int upgradeToUnlock = -1;
            public bool unlocked = true;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private DisplayModeComponent[] helpModeComponents;
        

        public float HideDelay = 5f;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void SetMode(DisplayModes mode)
        {
            if (mode == DisplayModes.All)
            {
                foreach (DisplayModeComponent dmc in helpModeComponents)
                {
                    dmc.helpObject.SetActive(dmc.unlocked || dmc.upgradeToUnlock == -1);
                }
                return;
            }
            
            for (int i = 0; i < helpModeComponents.Length; i++)
            {
                helpModeComponents[i].helpObject.SetActive(i+1 == (int)mode);
            }
            
        }

        public void UnlockHelpComponent(ModuleUpgrades upgradeScript)
        {
            bool[] upgrades = upgradeScript.GetUpgrades();
            
            foreach (DisplayModeComponent dmc in helpModeComponents)
            {
                if (dmc.upgradeToUnlock == -1 || upgrades[dmc.upgradeToUnlock])
                {
                    dmc.unlocked = true;
                }
            }
        }
    }
}
