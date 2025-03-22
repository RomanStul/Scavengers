using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module.Movement
{
    public class Movement : Player.Module.ModuleBaseScript
    {
        //================================================================CLASSES
        [Serializable]
        public class MovementConstants
        {
            public float Thrust;
            public float RotationThrust;
            public float MaxSpeed;
            public float fuelPerSecond;
            public float MaxFuel;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MovementConstants movementVariables;
        
        [SerializeField] protected GameObject moduleBody;

        [SerializeField] protected float currentFuel;
        //================================================================GETTER SETTER
        public float ThrustInput { get; set; } = 0f;
        public float RotationInput { get; set; } = 0f;
        public override void SetModule(Module module)
        {
            base.SetModule(module);
            Rigid = ModuleRef.GetMoveRb();
            RotationRigid = ModuleRef.GetRotateRb();
        }
        //================================================================FUNCTIONALITY

        protected Rigidbody2D Rigid, RotationRigid;

        public override void ApplyUpgrades()
        {
            //TODO space for fuel space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)movementVariables.MaxFuel, UI.UIController.BarsNames.FuelBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
            Refuel();
        }


        private void Update()
        {
            if (currentFuel <= 0)
            {
                return;
            }
            
            if (ThrustInput > 0 || ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Reverse) && ThrustInput < 0)
            {
                currentFuel -= Mathf.Abs(ThrustInput) * movementVariables.fuelPerSecond * Time.deltaTime;
                Rigid.AddForce(moduleBody.transform.up * (ThrustInput * movementVariables.Thrust));
                float speed = Rigid.velocity.magnitude;
                if (speed > movementVariables.MaxSpeed)
                {
                    Rigid.velocity = Rigid.velocity.normalized * movementVariables.MaxSpeed;
                }
            }

            if (Mathf.Abs(RotationInput) > 0)
            {
                currentFuel -= Mathf.Abs(RotationInput) *movementVariables.fuelPerSecond * Time.deltaTime;
                RotationRigid.AddTorque(-RotationInput * movementVariables.RotationThrust);
            }
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
        }

        public void Refuel(float amount = -1)
        {
            if (amount < 0)
            {
                currentFuel = movementVariables.MaxFuel;
            }
            else
            {
                currentFuel += amount;
            }
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
        }
    }
}
