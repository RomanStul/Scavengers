using System;
using UnityEngine;
using UnityEngine.UI;
using Player.Module;
using Player.Module.Movement;

namespace Player.UI.RepairRefuel
{
    public class RepairRefuel : UIWindow
    {
        public class RepairWindowParameters
        {
            public Module.Module moduleRef;
            public float repairCost;
            public float refuelCost;
        }
        
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Text repairCostText, refuelCostText;
        [SerializeField] private Button repairRefuelButton, repairButton, refuelButton;
        [SerializeField] private Slider repairAmountSlider, refuelAmountSlider;
        //================================================================GETTER SETTER

        public void SetParameters(RepairWindowParameters p)
        {
            parameters = p;
        }
        
        public void SetRepairAmount(Single amount)
        {
            repairAmountSlider.value = amount;
            repairAmount = amount;
            float repairCostTotal = repairAmount * parameters.repairCost;
            repairCostText.text = ((int)repairCostTotal).ToString();
            repairButton.interactable = (int)repairCostTotal <= (int)currency;
        }

        public void SetRefuelAmount(Single amount)
        {
            refuelAmountSlider.value = amount;
            refuelAmount = amount;
            float refuelCostTotal = refuelAmount * parameters.refuelCost;
            refuelCostText.text = ((int)refuelCostTotal).ToString();
            refuelButton.interactable = (int)refuelCostTotal <= (int)currency;
        }
        //================================================================FUNCTIONALITY

        private RepairWindowParameters parameters;
        private float refuelAmount, repairAmount;
        private float currency = 0;

        public override bool ToggleWindow()
        {
            HealthBar hb = parameters.moduleRef.GetScript<HealthBar>(Module.Module.ScriptNames.HealthBarScript);
            Movement mw = parameters.moduleRef.GetScript<Movement>(Module.Module.ScriptNames.MovementScript);
            
            ScaleRepairSlider(hb.GetMissingHealth());
            ScaleRefuelSlider(mw.GetMissingFuel());
            SetRepairAmount(0);
            SetRefuelAmount(0);
            
            Storage s = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript);
            currency = s.Currency;

            repairRefuelButton.interactable = mw.GetMissingFuel() * parameters.refuelCost + hb.GetMissingHealth() * parameters.repairCost <= s.Currency;

            return base.ToggleWindow();
        }
        
        
        public void Repair()
        {
            HealthBar hb = parameters.moduleRef.GetScript<HealthBar>(Module.Module.ScriptNames.HealthBarScript);
            
            float paid = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript).PayWithCurrency((int)(repairAmount * parameters.repairCost));
            currency -= paid;
            hb.HealHealth(paid / parameters.repairCost);
            SetRepairAmount(0);
            ScaleRepairSlider(hb.GetMissingHealth());
        }

        public void Refuel()
        {
            Movement mw =  parameters.moduleRef.GetScript<Movement>(Module.Module.ScriptNames.MovementScript);
            
            float paid = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript).PayWithCurrency((int)(refuelAmount * parameters.refuelCost));
            currency -= paid;
            mw.Refuel(paid / parameters.refuelCost);
            SetRefuelAmount(0);
            ScaleRefuelSlider(mw.GetMissingFuel());
        }

        private void ScaleRepairSlider(float missingHealth)
        {
            repairAmountSlider.maxValue = Mathf.Ceil(missingHealth);
        }

        private void ScaleRefuelSlider(float missingFuel)
        {
            refuelAmountSlider.maxValue = Mathf.Ceil(missingFuel);
        }

        public void RepairAndRefuel()
        {
            refuelAmount = refuelAmountSlider.maxValue;
            repairAmount = repairAmountSlider.maxValue;
            Repair();
            Refuel();
        }
        
    }
}
