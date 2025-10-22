using System;
using System.Collections.Generic;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Player.Module.Tools
{
    public class ToolHolder : BaseClass
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
        }
        
        //================================================================FUNCTIONALITY
        private int currentTool;
        private List<int> availableTools;


        public override void ApplyUpgrades()
        {
            GenerateToolList();
        }
        
        public void UseTool()
        {
            if (toolsCounts[availableTools[currentTool]].amount > 0)
            {
                //TODO use tool
                toolsCounts[availableTools[currentTool]].amount--;

                if (toolsCounts[availableTools[currentTool]].amount == 0)
                {
                    availableTools.RemoveAt(currentTool);
                    ChangeTool(1);
                }
            }
        }
        
        
        //creates list of tools that have more that one in storage and add images to UI
        public void GenerateToolList()
        {
            availableTools = new List<int>();
            for (int i = 0; i < toolsCounts.Length; i++)
            {
                if (toolsCounts[i].amount > 0)
                {
                    availableTools.Add(i);
                }
            }
        }

        public void ChangeTool(int offset)
        {
            if (availableTools.Count == 0)
            {
                //TODO hide UI
                return;
            }
            currentTool += offset;
            if (currentTool >= availableTools.Count)
            {
                currentTool = 0;
            }

            if (currentTool < 0)
            {
                currentTool = availableTools.Count - 1;
            }
            //TODO update UI
        }
        
    }
}
