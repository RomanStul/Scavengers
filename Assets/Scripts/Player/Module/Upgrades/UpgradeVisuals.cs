using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module.Upgrades
{
    public class UpgradeVisuals : ModuleBaseScript
    {
        //================================================================CLASSES
        [Serializable]
        public class VisualChange
        {
            public ModuleUpgrades.Ups[] neededUps;
            public GameObject[] toActivate;
            public GameObject[] toDeactivate;
            public bool changed;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private VisualChange[] _upgradeChanges;

        [SerializeField] private SpriteRenderer acidRenderer;

        [SerializeField] private Sprite[] acidTextures;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public override void ApplyUpgrades()
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
            foreach (var condition in _upgradeChanges[i].neededUps)
            {
                if (!ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(condition))
                {
                    return false; 
                }
            }

            return true;
        }

        #region effects

        public void ShowAcidTexture()
        {
            acidRenderer.gameObject.SetActive(true);
        }

        public float ChangeAcidTexture(float value)
        {
            if ((int)value > acidTextures.Length)
            {
                return acidTextures.Length;
            }

            if ((int)value == 0)
            {
                acidRenderer.sprite = null;
                return 0;
            }
            acidRenderer.sprite = acidTextures[(int)Mathf.Floor(value) - 1];
            return value;
        }

        #endregion
    }
}
