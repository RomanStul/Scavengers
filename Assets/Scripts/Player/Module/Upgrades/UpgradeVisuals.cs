using System;
using UnityEngine;

namespace Player.Module.Upgrades
{
    public class UpgradeVisuals : ModuleBaseScript
    {
        //================================================================CLASSES
        [Serializable]
        public class VisualChange
        {
            public Upgrades.Ups[] NeededUps;
            public GameObject[] toActivate;
            public GameObject[] toDeactivate;
            public bool changed;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private VisualChange[] _upgradeChanges;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public override void SetModule(Module module)
        {
            base.SetModule(module);
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < _upgradeChanges.Length; i++)
            {
                if (!_upgradeChanges[i].changed && CheckConditions(i))
                {
                    _upgradeChanges[i].changed = true;
                    foreach (GameObject a in _upgradeChanges[i].toActivate)
                    {
                        a.SetActive(true);
                    }

                    foreach (GameObject a in _upgradeChanges[i].toDeactivate)
                    {
                        a.SetActive(false);
                    }
                }
            }
        }

        private bool CheckConditions(int i)
        {
            foreach (var condition in _upgradeChanges[i].NeededUps)
            {
                if (!ModuleRef.GetScript<Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(condition))
                {
                    return false; 
                }
            }

            return true;
        }
    }
}
