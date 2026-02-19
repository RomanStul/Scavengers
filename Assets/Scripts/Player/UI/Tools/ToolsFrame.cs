using ScriptableObjects.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.Tools
{
    public class ToolsFrame : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolSO toolObject;

        [SerializeField] private Image icon;
        [SerializeField] private Text count;
        //================================================================GETTER SETTER
        public void SetToolsShop(ToolsShopWindow shopWindow)
        {
            _toolsShopWindow = shopWindow;
        }

        public void SetCount(int numberOfItems)
        {
            count.text = count.ToString();
        }

        public ToolSO GetTool()
        {
            return toolObject;
        }
        //================================================================FUNCTIONALITY
        private ToolsShopWindow _toolsShopWindow;


        public void InitializeButton(ToolSO tool, int toolCount, ToolsShopWindow shopWindow)
        {
            toolObject = tool;
            icon.sprite = tool.icon;
            count.text = toolCount.ToString();
            _toolsShopWindow = shopWindow;
        }
        
        public void ClickedTool()
        {
            _toolsShopWindow.DisplayTool(toolObject);
        }
    }
}
