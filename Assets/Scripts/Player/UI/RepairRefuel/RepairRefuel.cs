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
        
        public void SetRepairAmount(Single amount = -1)
        {
            if (Mathf.Approximately(amount, -1))
            {
                amount = CalculateMaxRepair();
            }

            amount = Mathf.Ceil(amount);
            repairAmount = amount;
            repairAmountSlider.value = amount;
            repairCostText.text = ((int)amount).ToString();
            repairButton.interactable = amount <= (int)currency && repairAmount != 0;
        }

        public void SetRefuelAmount(Single amount = -1)
        {
            if (Mathf.Approximately(amount, -1))
            {
                amount = CalculateMaxRefuel();
            }
            
            amount = Mathf.Ceil(amount);
            refuelAmount = amount;
            refuelAmountSlider.value = amount;
            refuelCostText.text = ((int)amount).ToString();
            refuelButton.interactable = amount <= (int)currency && refuelAmount != 0;
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
            
            Storage s = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript);
            currency = s.Currency;

            repairRefuelButton.interactable = mw.GetMissingFuel() * parameters.refuelCost + hb.GetMissingHealth() * parameters.repairCost <= s.Currency;
            
            SetRepairAmount();
            SetRefuelAmount();

            return base.ToggleWindow();
        }

        private Single CalculateMaxRefuel()
        {
            Movement mw = parameters.moduleRef.GetScript<Movement>(Module.Module.ScriptNames.MovementScript);

            return Mathf.Min(currency, (parameters.refuelCost) * mw.GetMissingFuel());
        }
        
        private Single CalculateMaxRepair()
        {
            HealthBar hb = parameters.moduleRef.GetScript<HealthBar>(Module.Module.ScriptNames.HealthBarScript);

            return Mathf.Min(Mathf.Min(currency, (parameters.repairCost) * hb.GetMissingHealth()));
        }
        
        
        public void Repair()
        {
            HealthBar hb = parameters.moduleRef.GetScript<HealthBar>(Module.Module.ScriptNames.HealthBarScript);
            
            float paid = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript).PayWithCurrency((int)(repairAmount));
            currency -= paid;
            hb.HealHealth(paid / parameters.repairCost);
            SetRepairAmount(0);
            ScaleRepairSlider(hb.GetMissingHealth());
            SetRefuelAmount();
        }

        public void Refuel()
        {
            Movement mw =  parameters.moduleRef.GetScript<Movement>(Module.Module.ScriptNames.MovementScript);
            
            float paid = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript).PayWithCurrency((int)(refuelAmount));
            currency -= paid;
            mw.Refuel(paid / parameters.refuelCost);
            SetRefuelAmount(0);
            ScaleRefuelSlider(mw.GetMissingFuel());
            SetRepairAmount();
        }

        private void ScaleRepairSlider(float missingHealth)
        {
            repairAmountSlider.maxValue = Mathf.Ceil(missingHealth*parameters.repairCost);
        }

        private void ScaleRefuelSlider(float missingFuel)
        {
            refuelAmountSlider.maxValue = Mathf.Ceil(missingFuel*parameters.refuelCost);
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
