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
        [SerializeField] private Button repairRefuelButton;
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
            repairCostText.text = ((int)(repairAmount * parameters.repairCost)).ToString();
        }

        public void SetRefuelAmount(Single amount)
        {
            refuelAmountSlider.value = amount;
            refuelAmount = amount;
            refuelCostText.text = ((int)(refuelAmount * parameters.refuelCost)).ToString();
        }
        //================================================================FUNCTIONALITY

        private RepairWindowParameters parameters;
        private float refuelAmount, repairAmount;

        public override bool ToggleWindow()
        {
            HealthBar hb = parameters.moduleRef.GetScript<HealthBar>(Module.Module.ScriptNames.HealthBarScript);
            Movement mw = parameters.moduleRef.GetScript<Movement>(Module.Module.ScriptNames.MovementScript);
            
            ScaleRepairSlider(hb.GetMissingHealth());
            ScaleRefuelSlider(mw.GetMissingFuel());
            SetRepairAmount(0);
            SetRefuelAmount(0);
            
            Storage s = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript);

            repairRefuelButton.interactable = mw.GetMissingFuel() * parameters.refuelCost + hb.GetMissingHealth() * parameters.repairCost <= s.Currency;

            return base.ToggleWindow();
        }
        
        
        public void Repair()
        {
            HealthBar hb = parameters.moduleRef.GetScript<HealthBar>(Module.Module.ScriptNames.HealthBarScript);
            
            float repaired = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript).PayWithCurrency((int)(repairAmount * parameters.repairCost)) / parameters.repairCost;
            hb.HealHealth(repaired);
            SetRepairAmount(0);
            ScaleRepairSlider(hb.GetMissingHealth());
        }

        public void Refuel()
        {
            Movement mw =  parameters.moduleRef.GetScript<Movement>(Module.Module.ScriptNames.MovementScript);
            
            float refueled = parameters.moduleRef.GetScript<Storage>(Module.Module.ScriptNames.StorageScript).PayWithCurrency((int)(refuelAmount * parameters.refuelCost)) / parameters.refuelCost;
            mw.Refuel(refueled);
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
