using System;
using System.Collections;
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

        [Serializable]
        public class DashConstants
        {
            public float DashFuelConsumption;
            public float DashPower;
            public float DashLength;
            public float DashCooldown;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MovementConstants movementVariables;
        [SerializeField] protected DashConstants dashConstants;
        
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
        private bool takeInput = true, dashReady = false;

        public override void ApplyUpgrades()
        {
            //TODO space for fuel space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)movementVariables.MaxFuel, UI.UIController.BarsNames.FuelBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
            ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Dash);
            Refuel();
        }


        private void Update()
        {
            if (!takeInput)
            {
                return;
            }
            
            if (currentFuel <= 0)
            {
                return;
            }
            
            if (ThrustInput > 0 || ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Reverse) && ThrustInput < 0)
            {
                currentFuel -= Mathf.Abs(ThrustInput) * movementVariables.fuelPerSecond * Time.deltaTime;
                Rigid.AddForce(moduleBody.transform.up * (ThrustInput * movementVariables.Thrust * Time.deltaTime));
                float speed = Rigid.velocity.magnitude;
                //TODO make it so that input interacts with dash momentum intuitively
                if (speed > movementVariables.MaxSpeed)
                {
                    Rigid.velocity = Rigid.velocity.normalized * movementVariables.MaxSpeed;
                }
            }

            if (Mathf.Abs(RotationInput) > 0)
            {
                currentFuel -= Mathf.Abs(RotationInput) *movementVariables.fuelPerSecond * Time.deltaTime;
                RotationRigid.AddTorque(-RotationInput * movementVariables.RotationThrust * Time.deltaTime);
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

        public void Dash()
        {
            if (currentFuel >= dashConstants.DashFuelConsumption && dashReady)
                StartCoroutine(DashExecute());
        }

        private IEnumerator DashExecute()
        {
            dashReady = false;
            takeInput = false;

            currentFuel -= dashConstants.DashFuelConsumption;
            Rigid.AddForce(moduleBody.transform.up * (dashConstants.DashPower));
            
            yield return new WaitForSeconds(dashConstants.DashLength);
            takeInput = true;
            yield return new WaitForSeconds(dashConstants.DashCooldown - dashConstants.DashLength);
            dashReady = true;
        }

    }
}
