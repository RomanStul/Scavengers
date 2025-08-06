using System;
using Player.Module.Upgrades;
using ScriptableObjects.Upgrade;
using UnityEngine;

namespace Player.UI.Upgrades
{
    public class UpgradeWindowController : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] private UpgradeSO[] upgrades;
        [SerializeField] private UpgradeSelectButton buttonPrefab;


        [SerializeField] private Transform buttonContainer; 

        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private UpgradeSelectButton[] buttons;

        public void SetUpUpgrades(ModuleUpgrades.UpgradeObject[] upgradesInfo)
        {
            buttons = new UpgradeSelectButton[upgrades.Length];
            
            for (int i = 0; i < upgrades.Length; i++)
            {
                buttons[i] = Instantiate(buttonPrefab, buttonContainer);
                buttons[i].SetText(upgrades[i].name);
            }
            
            for (int i = 0; i < upgrades.Length; i++)
            {
                Debug.Log(upgrades[i].name);
                if (upgradesInfo[(int)upgrades[i].tag].unlocked)
                {
                    buttons[i].gameObject.SetActive(false);
                    continue;
                }
                for (int j = 0; j < upgrades[i].neededUpgrades.Length; j++)
                {
                    if (!upgradesInfo[(int)upgrades[i].neededUpgrades[j]].unlocked)
                    {
                        Debug.Log(buttons[i]);
                        buttons[i].gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
