using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Module.Tools
{
    public class ToolHolder : BaseClass
    {
        //================================================================CLASSES

        public enum ToolType
        {
            
        }

        [Serializable]
        public class ToolArrayRow
        {
            public ToolArrayRow(string toolName, int initialAmount)
            {
                name = toolName;
                amount = initialAmount;
            }
            
            [HideInInspector] public string name;
            public int amount;
        }
        
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private ToolArrayRow[] toolsCounts;
        //================================================================GETTER SETTER

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

        public void GenerateToolsCounts()
        {
            ToolArrayRow[] newCounts = new ToolArrayRow[Enum.GetValues(typeof(ToolType)).Length];
            for (int i = 0; i < Mathf.Min(toolsCounts.Length, newCounts.Length); i++)
            {
                newCounts[i] = toolsCounts[i];
            }

            if (toolsCounts.Length < newCounts.Length)
            {
                for (int i = toolsCounts.Length; i < newCounts.Length; i++)
                {
                    newCounts[i] = new ToolArrayRow(Enum.GetName(typeof(ToolType), i), 0);
                }
            }
        }
        
        //================================================================FUNCTIONALITY
        private int currentTool;
        private List<int> availableTools;

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
