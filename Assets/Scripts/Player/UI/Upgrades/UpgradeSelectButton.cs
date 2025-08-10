using ScriptableObjects.Upgrade;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.Upgrades
{
    public class UpgradeSelectButton : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] private Text buttonText;
        [SerializeField] private UpgradeSO upgradeSo;
        
        //================================================================GETTER SETTER

        public void SetText(string text)
        {
            this.buttonText.text = text;
        }


        public void SetController(UpgradeWindowController c)
        {
            controller = c;
        }

        public void SetUpgrade(UpgradeSO upgrade)
        {
            this.upgradeSo = upgrade;
        }

        public UpgradeSO GetUpgrade()
        {
            return upgradeSo;
        }
        //================================================================FUNCTIONALITY
        private UpgradeWindowController controller;

        public void Click()
        {
            controller.ClickedUpgrade(this);
        }
    }
}
