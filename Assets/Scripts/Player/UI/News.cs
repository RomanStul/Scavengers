using ScriptableObjects.Day;
using story;
using TMPro;
using UnityEngine;

namespace Player.UI
{
    public class News : UIWindow
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private TextMeshProUGUI headline, mainText, economics;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public override bool ToggleWindow()
        {
            if (base.ToggleWindow())
            {
                SetNews();
                return true;
            }
            return false;
        } 
        
        public void SetNews()
        {
            StoryManager.News newsObject = StoryManager.instance.GetNewsForDay();

            headline.text = newsObject.Title;
            mainText.text = newsObject.MainText;
            WriteEconomics();
        }

        private void WriteEconomics()
        {
            DaySO currentDaySO = StoryManager.instance.GetDay();
            OrePriceChanges[] priceChanges = currentDaySO.priceChanges;

            foreach (OrePriceChanges prCh in priceChanges)
            {
                CreateEconomicsLine(prCh.item.ToString().Replace('_', ' '), prCh.multiplier);
            }

            if (currentDaySO.refuelCostMultiplier > 0)
            {
                CreateEconomicsLine("Fuel", currentDaySO.refuelCostMultiplier);
            }

            if (currentDaySO.repairCostMultiplier > 0)
            {
                CreateEconomicsLine("Repair", currentDaySO.repairCostMultiplier);
            }

            economics.text += "\nTax today: " + StoryManager.instance.GetStartOfDayPayment();
        }

        private void CreateEconomicsLine(string name, float mult)
        {
            if(mult > 1)
                economics.text += "\n" + name + " price increased,";
            else
            {
                economics.text += "\n" + name + " price decreased.";
            }
        }
    }
}
