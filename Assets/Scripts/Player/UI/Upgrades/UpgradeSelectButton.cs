using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.Upgrades
{
    public class UpgradeSelectButton : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        [SerializeField] private Text buttonText;
        //================================================================GETTER SETTER

        public void SetText(string text)
        {
            this.buttonText.text = text;
        }
        
        //================================================================FUNCTIONALITY
    }
}
