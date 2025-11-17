using System;
using System.Collections.Generic;
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
        
        //================================================================FUNCTIONALITY
        private int currentTool;


        public override void ApplyUpgrades()
        {

        }
        
        public void UseTool()
        {
            if (toolsCounts[currentTool].amount > 0)
            {
                ModuleTool mt = Instantiate(toolsCounts[currentTool].tool.executeObject) as ModuleTool;
                mt.Use(ModuleRef);
                toolsCounts[currentTool].amount--;

                if (toolsCounts[currentTool].amount == 0)
                {
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
            for (int i = toolsCounts.Length; i != toolsCounts.Length + offset * toolsCounts.Length; i+= offset)
            {
                if (toolsCounts[(currentTool + i) % toolsCounts.Length].amount != 0)
                {
                    currentTool = (currentTool + i) % toolsCounts.Length;
                    ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(toolsCounts[currentTool].tool, toolsCounts[currentTool].amount);
                    return;
                }
            }

            currentTool = -1;
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetTool(null, -1);

        }
        
    }
}
