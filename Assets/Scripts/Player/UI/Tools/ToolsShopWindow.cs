using System;
using System.Collections.Generic;
using System.Linq;
using Player.Module;
using Player.Module.Tools;
using Player.Module.Upgrades;
using ScriptableObjects.Tools;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Player.UI.Tools
{
    public class ToolsShopWindow : UIWindow
    {
        //================================================================CLASSES
        [Serializable]
        public class RightSide
        {
            public Text headline;
            public Text description;
            public RectTransform priceHolder;
            public UpgradeCost costPrefab;
            public Button buyButton;
            public Image icon;
            public Sprite currencyIcon;

            public List<UpgradeCost> costs = new List<UpgradeCost>();
        }

        [Serializable]
        public class LeftSide
        {
            public ToolsFrame toolFramePrefab;
            public RectTransform toolFrameContainer;
            
        }

        [Serializable]
        public class ToolsReferences
        {
            public ToolSO tool;
            public bool unlocked;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolsReferences[] tools;
        [SerializeField] private RightSide rightSideReferences;
        [SerializeField] private LeftSide leftSideReferences;
        
        //================================================================GETTER SETTER
        public void SetStorage(Storage moduleStorage)
        {
            storage = moduleStorage;
        }

        public void SetToolHolder(ToolHolder moduleToolHolder)
        {
            toolHolder = moduleToolHolder;
        }
        
        //================================================================FUNCTIONALITY
        private ToolsFrame[] toolFrames;
        
        private Storage storage;
        private ToolHolder toolHolder;

        private ToolSO currentTool;


        public override bool ToggleWindow()
        {
            
            bool opened = base.ToggleWindow();
            if (!opened)
            {
                ResetRightSide();
            }
            
            return opened;
        }

        private void ResetRightSide()
        {
            rightSideReferences.description.text = "";
            rightSideReferences.headline.text = "";
            rightSideReferences.icon.sprite = null;
            rightSideReferences.icon.color = new Color(0, 0, 0, 0);
            for (int i = 0; i < rightSideReferences.costs.Count; i++)
            {
                rightSideReferences.costs[i].gameObject.SetActive(false);
            }
            rightSideReferences.buyButton.gameObject.SetActive(false);
        }

        public void DisplayTool(ToolSO toolToDisplay)
        {
            
            currentTool = toolToDisplay;
            
            rightSideReferences.headline.text = toolToDisplay.name.Replace("_", " ");
            rightSideReferences.description.text = toolToDisplay.description;
            rightSideReferences.icon.sprite = toolToDisplay.icon;
            rightSideReferences.icon.color = new Color(255,255,255,255);
            rightSideReferences.buyButton.gameObject.SetActive(true);
            rightSideReferences.buyButton.interactable = true;

            int usedCostsCount = 0;
            
            for (int i = 0; i < toolToDisplay.neededItems.Length; i++)
            {

                if (rightSideReferences.costs.Count <= usedCostsCount)
                {
                    CreateCost();
                }



                rightSideReferences.costs[usedCostsCount].gameObject.SetActive(true);
                rightSideReferences.costs[usedCostsCount].SetIcon(toolToDisplay.neededItems[i].item.image);
                rightSideReferences.costs[usedCostsCount].SetCostAmount(toolToDisplay.neededItems[i].amount);
                usedCostsCount++;
            }


            if (toolToDisplay.price > 0)
            {
                if (rightSideReferences.costs.Count == 0 || rightSideReferences.costs.Count <= usedCostsCount)
                {
                    CreateCost();
                }
                
                rightSideReferences.costs[usedCostsCount].SetCostAmount(toolToDisplay.price);
                rightSideReferences.costs[usedCostsCount].SetIcon(rightSideReferences.currencyIcon);
                
            }
            
            rightSideReferences.buyButton.gameObject.SetActive(true);
            
            CheckIfCanBuy();
        }

        public void CheckIfCanBuy()
        {
            rightSideReferences.buyButton.interactable = false;
            
            for (int i = 0; i < currentTool.neededItems.Length; i++)
            {
                if (!storage.HasAtleast((int)currentTool.neededItems[i].item.itemType, currentTool.neededItems[i].amount))
                {
                    rightSideReferences.buyButton.interactable = false;
                    return;
                }
            }

            if (currentTool.price > 0)
            {
                rightSideReferences.buyButton.interactable = storage.Currency >= currentTool.price;
            }
            
        }

        private void CreateCost()
        {
            rightSideReferences.costs.Add(Instantiate(rightSideReferences.costPrefab, rightSideReferences.priceHolder));
        }

        public void CreateToolButtons()
        {
            if (toolFrames != null)
            {
                return;
            }
            
            toolFrames = new ToolsFrame[tools.Length];
            
            for (int i = 0; i < tools.Length; i++)
            {
                toolFrames[i] = Instantiate(leftSideReferences.toolFramePrefab, leftSideReferences.toolFrameContainer);
                toolFrames[i].gameObject.SetActive(tools[i].unlocked);
                toolFrames[i].InitializeButton(tools[i].tool, toolHolder.GetAmountOfToolType(tools[i].tool.toolType), this);
            }
        }

        public void UnlockTools(ModuleUpgrades.UpgradeObject[] moduleUpgradeObject)
        {
            for (int i = 0; i < toolFrames.Length; i++)
            {
                if (toolFrames[i].GetTool().unlockUpgrade == ModuleUpgrades.Ups.Portal_passkey ||moduleUpgradeObject[(int)toolFrames[i].GetTool().unlockUpgrade].unlocked)
                {
                    toolFrames[i].gameObject.SetActive(true);
                }
            }
        }

        public void BuyTool()
        {
            for (int i = 0; i < currentTool.neededItems.Length; i++)
            {
                storage.RemoveItem(currentTool.neededItems[i].item, currentTool.neededItems[i].amount);
            }

            storage.PayWithCurrency(currentTool.price, false);
            
            toolFrames[(int)currentTool.toolType].SetCount(toolHolder.AddTool(currentTool, 1));
            
            CheckIfCanBuy();
        }

        public void SetToolCount(ToolSO.ToolType type, int amount)
        {
            foreach (var toolFrame in toolFrames)
            {
                if (toolFrame.GetTool().toolType == type)
                {
                    toolFrame.SetCount(amount);
                }
            }
        }

    }
}
