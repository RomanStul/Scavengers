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
        [SerializeField] private TextMeshProUGUI headline, mainText, economics, secondaryHeadline, secondaryText;
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

            if (newsObject == null)
            {
                ToggleWindow();
                return;
            }
            headline.text = newsObject.Title;
            mainText.text = newsObject.MainText;
            secondaryText.text = newsObject.SecondaryText;
            secondaryHeadline.text = newsObject.SecondaryTitle;
            WriteEconomics();
        }

        private void WriteEconomics()
        {
            economics.text = "";
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

            economics.text += "\nQuota today: " + StoryManager.instance.GetStartOfDayPayment();
        }

        private void CreateEconomicsLine(string name, float mult)
        {
            if(mult > 1)
                economics.text += "\n" + name + " price increased";
            else
            {
                economics.text += "\n" + name + " price decreased";
            }
        }
    }
}
