using System;
using System.Collections.Generic;
using Player.Module;
using Player.Module.Upgrades;
using ScriptableObjects.Upgrade;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Player.UI.Upgrades
{
    
    public class UpgradeWindowController : UIWindow
    {
        //================================================================CLASSES
        
        [Serializable]
        private class LeftSide
        {
            public UpgradeSelectButton buttonPrefab;
            public Transform buttonContainer;
        }

        [Serializable]
        private class RightSide
        {
            public Text upgradeName;
            public Text upgradeDescription;
            public UpgradeCost costPrefab;
            public Transform costContainer;
            public Sprite currencyIcon;
            public Button installButton;
            public UnityEvent<int> installEvent;
        }
        
        //================================================================EDITOR VARIABLES

        [SerializeField] private UpgradeSO[] upgrades;
        [SerializeField] private LeftSide leftSide;
        [SerializeField] private RightSide rightSide;

        //================================================================GETTER SETTER

        public void SetStorage(Storage s)
        {
            storage = s;
        }
        
        //================================================================FUNCTIONALITY
        private UpgradeSelectButton[] buttons = Array.Empty<UpgradeSelectButton>();
        private List<UpgradeCost> costs = new List<UpgradeCost>();
        private UpgradeSelectButton selectedUpgradeButton;
        private Storage storage;

        public override bool ToggleWindow()
        {
            if (base.ToggleWindow())
            {
                return true;
            }

            ResetRightSide();
            return false;
        }

        public override void CloseWindow()
        {
            base.CloseWindow();
            ResetRightSide();
        }
        
        public void SetUpUpgrades(ModuleUpgrades.UpgradeObject[] upgradesInfo)
        {
            if (buttons.Length == 0)
            {
                buttons = new UpgradeSelectButton[upgrades.Length];
                
                for (int i = 0; i < upgrades.Length; i++)
                {
                    if (upgradesInfo[(int)upgrades[i].tag].unlocked)
                    {
                        continue;
                    }
                    buttons[i] = Instantiate(leftSide.buttonPrefab, leftSide.buttonContainer);
                    buttons[i].SetText(upgrades[i].name);
                    buttons[i].SetController(this);
                    buttons[i].SetUpgrade(upgrades[i]);
                }
            }
            
            for (int i = 0; i < upgrades.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
                for (int j = 0; j < upgrades[i].neededUpgrades.Length; j++)
                {
                    if (!upgradesInfo[(int)upgrades[i].neededUpgrades[j]].unlocked)
                    {
                        buttons[i].gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }

        public void ClickedUpgrade(UpgradeSelectButton upgradeButton)
        {
            
            selectedUpgradeButton = upgradeButton;
            UpgradeSO upgrade = upgradeButton.GetUpgrade();
            rightSide.installButton.transform.gameObject.SetActive(true);
            rightSide.upgradeName.text = upgrade.tag.ToString().Replace("_", " ");;
            rightSide.upgradeDescription.text = upgrade.description;
            bool canUpgrade = true;
            
            int neededCost = upgrade.neededItems.Length + upgrade.cost == 0 ? 0 : 1;
            
            
            int i = 0;
            for (;i < upgrade.neededItems.Length; i++)
            {
                if (i == costs.Count)
                {
                    CreateCostField();
                }
                
                costs[i].gameObject.SetActive(true);
                costs[i].SetCostAmount(upgrade.neededItems[i].amount);
                bool hasEnough = storage.HasAtleast((int)upgrade.neededItems[i].item.itemType, upgrade.neededItems[i].amount);
                canUpgrade = canUpgrade && hasEnough;
                costs[i].ShowResourceAvailable(hasEnough);
                costs[i].SetIcon(upgrade.neededItems[i].item.image);
            }

            if (upgrade.cost != 0)
            {
                if (i == costs.Count)
                {
                    CreateCostField();
                }    
                costs[i].SetCostAmount(upgrade.cost);
                costs[i].gameObject.SetActive(true);
                costs[i].ShowResourceAvailable(storage.Currency >= upgrade.cost);
                canUpgrade = canUpgrade && storage.Currency >= upgrade.cost;
                costs[i].SetIcon(rightSide.currencyIcon);
                i++;
            }

            for (; i < costs.Count; i++)
            {
                costs[i].gameObject.SetActive(false);
            }


            rightSide.installButton.interactable = canUpgrade;
        }


        private void CreateCostField()
        {
            costs.Add(Instantiate(rightSide.costPrefab, rightSide.costContainer));
        }

        private void ResetRightSide()
        {
            selectedUpgradeButton = null;
            rightSide.upgradeName.text = "";
            rightSide.upgradeDescription.text = "";
            rightSide.installButton.gameObject.SetActive(false);
            rightSide.installButton.interactable = false;
            for (int i = 0; i < costs.Count; i++)
            {
                costs[i].gameObject.SetActive(false);
            }
        }

        public void InstallUpgrade()
        {
            if (selectedUpgradeButton == null)
            {
                return;
            }
            
            storage.PayWithCurrency(selectedUpgradeButton.GetUpgrade().cost);
            foreach (var t in selectedUpgradeButton.GetUpgrade().neededItems)
            {
                storage.RemoveItem(t.item, t.amount);
            }
            
            rightSide.installEvent.Invoke((int)selectedUpgradeButton.GetUpgrade().tag);
            Destroy(selectedUpgradeButton.gameObject);
            
            ResetRightSide();
        }
    }
}
