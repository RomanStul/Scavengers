using UnityEngine;

namespace Player.UI
{
    public class UIController : Module.BaseClass
    {
        public enum BarsNames
        {
            HealthBar,
            FuelBar,
            StorageBar
        }
        //================================================================CLASSES
    
        //================================================================EDITOR VARIABLES
        [SerializeField] private Bar HealthBar, FuelBar, StorageBar;
        //================================================================GETTER SETTER

        //================================================================FUNCTIONALITY

        public void SetBar(int value, BarsNames barName, bool isMax = false)
        {
            Bar targetBar = null;

            switch (barName)
            {
                case BarsNames.HealthBar:
                    targetBar = HealthBar;
                    break;
                case BarsNames.FuelBar:
                    targetBar = FuelBar;
                    break;
                case BarsNames.StorageBar:
                    targetBar = StorageBar;
                    break;
                default:
                    return;
            }

            if (isMax)
            {
                targetBar.SetMaxValue(value);
            }
            else
            {
                targetBar.SetValue(value);
            }
        }
    }
}
