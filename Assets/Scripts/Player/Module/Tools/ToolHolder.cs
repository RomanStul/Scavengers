using System;
using System.Collections.Generic;
using HelpScripts;
using Player.UI;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Player.Module.Tools
{
    public class ToolHolder : ModuleBaseScript
    {
        //================================================================CLASSES

        [Serializable]
        public class ToolArrayRow //class to have named objects in inspector
        {
            public ToolArrayRow(string toolName, int initialAmount)
            {
                name = toolName;
                amount = initialAmount;
            }

            [HideInInspector] public string name;
            public int amount;
            public ToolSO tool;
        }

        //================================================================EDITOR VARIABLES

        [SerializeField] private ToolArrayRow[] toolsCounts;
        [SerializeField] private Transform toolSpawnPoint;
        [SerializeField] private SpriteRenderer toolPlaceholder;

        //================================================================GETTER SETTER


        //For generating array of tools after adding new ones, called from editor script
        public void GenerateToolsCounts()
        {
            ToolArrayRow[] newCounts = new ToolArrayRow[Enum.GetValues(typeof(ToolSO.ToolType)).Length];
            for (int i = 0; i < Mathf.Min(toolsCounts.Length, newCounts.Length); i++)
            {
                newCounts[i] = toolsCounts[i];
            }

            if (toolsCounts.Length < newCounts.Length)
            {
                for (int i = toolsCounts.Length; i < newCounts.Length; i++)
                {
                    newCounts[i] = new ToolArrayRow(Enum.GetName(typeof(ToolSO.ToolType), i), 0);
                }
            }

            toolsCounts = newCounts;
        }

        public int GetAmountOfToolType(ToolSO.ToolType toolType)
        {
            return toolsCounts[(int)toolType].amount;
        }

        public int[] GetToolAmounts()
        {
            int[] amounts = new int[toolsCounts.Length];
            for (int i = 0; i < toolsCounts.Length; i++)
            {
                amounts[i] = toolsCounts[i].amount;
            }

            return amounts;
        }

        public void SetToolAmounts(int[] amounts)
        {
            for (int i = 0; i < toolsCounts.Length; i++)
            {
                toolsCounts[i].amount = amounts[i];
            }

        }

        //================================================================FUNCTIONALITY
        private int currentTool;
        private bool ShowPlaceholder = false;


        public override void ApplyUpgrades()
        {
            currentTool = 0;
            ChangeTool(1);
            if(currentTool != -1)
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(toolsCounts[currentTool].tool, toolsCounts[currentTool].amount);
        }

        private void FixedUpdate()
        {
            if (ShowPlaceholder && currentTool != -1)
            {
                RaycastHit2D rh2d = MyRaycast.RaycastCollider(toolSpawnPoint.position, toolSpawnPoint.position - transform.position, 4f);
                if(rh2d.collider != null)
                    toolPlaceholder.transform.position = rh2d.point;
                else
                    toolPlaceholder.transform.position = toolSpawnPoint.position + (toolSpawnPoint.position - transform.position).normalized * 4f;
                
                toolPlaceholder.sprite = toolsCounts[currentTool].tool.icon;
            }
        }

        public void UseTool()
        {
            if (currentTool != -1 && toolsCounts[currentTool].amount > 0)
            {
                ModuleTool mt = Instantiate(toolsCounts[currentTool].tool.executeObject, toolSpawnPoint.position,
                    transform.rotation) as ModuleTool;
                mt.Use(ModuleRef);
                toolsCounts[currentTool].amount--;
                
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetToolCount(toolsCounts[currentTool].tool.toolType, toolsCounts[currentTool].amount);

                if (toolsCounts[currentTool].amount == 0)
                {
                    if (ShowPlaceholder)
                    {
                        ShowToolTutorial(false);
                    }
                    ChangeTool(1);
                }
                else
                {
                    ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(toolsCounts[currentTool].tool, toolsCounts[currentTool].amount);
                }
            }
        }


        public void ChangeTool(int offset)
        {
            int lastTool = currentTool;
            for (int i = toolsCounts.Length + offset; i != toolsCounts.Length + offset * toolsCounts.Length; i += offset)
            {
                if (toolsCounts[(currentTool + i) % toolsCounts.Length].amount != 0)
                {
                    currentTool = (currentTool + i) % toolsCounts.Length;
                    ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(toolsCounts[currentTool].tool, toolsCounts[currentTool].amount);
                    return;
                }
            }

            if (lastTool == -1 || toolsCounts[lastTool].amount == 0)
            {
                currentTool = -1;
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(null, -1);
            }
            else
            {
                currentTool = lastTool;
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(toolsCounts[currentTool].tool, toolsCounts[currentTool].amount);
            }

        }

        public int AddTool(ToolSO tool, int amount)
        {
            toolsCounts[(int)tool.toolType].amount += amount;
            if (currentTool == -1)
            {
                ChangeTool(1);
            }

            if (currentTool == (int)tool.toolType)
            {
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(toolsCounts[currentTool].tool, toolsCounts[currentTool].amount);
            }
            return toolsCounts[(int)tool.toolType].amount;
        }

        public bool ShowToolTutorial(bool show)
        {
            if (!show)
            {
                ShowPlaceholder = show;
                toolPlaceholder.sprite = null;
                return true;
            }
            
            if (currentTool == -1)
            {
                return false;
            }
            ShowPlaceholder = show;
            return true;

        }

}
}
