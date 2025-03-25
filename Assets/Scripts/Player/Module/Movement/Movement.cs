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

        [Serializable]

        public class StopConstants
        {
            public float fuelConsumptionMultiplier;
            public float stopLength;
            public float stopCooldown;
        }

        [Serializable]

        public class DashSidewaysConstants
        {
            public float strength;
            public float fuelConsumption;
            public float length;
            public float cooldown;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MovementConstants movementVariables;
        [SerializeField] protected DashConstants dashConstants;
        [SerializeField] protected StopConstants stopConstants;
        [SerializeField] protected DashSidewaysConstants dashSidewaysConstants;
        
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

        private bool takeInput = true,
            dashReady = false,
            reverseAvailable = false,
            stopReady = false,
            dashSidewaysReady = false;

        public override void ApplyUpgrades()
        {
            //TODO space for fuel space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)movementVariables.MaxFuel, UI.UIController.BarsNames.FuelBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
            dashReady = ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Dash);
            reverseAvailable = ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Reverse);
            stopReady = ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Stop);
            dashSidewaysReady = ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.DashSideWays);

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
            
            if (ThrustInput > 0 || reverseAvailable && ThrustInput < 0)
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
            if (currentFuel >= dashConstants.DashFuelConsumption && dashReady && takeInput)
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

        public void Stop()
        {
            if (stopReady && takeInput)
            {
                StartCoroutine(StopExecute());
            }
        }

        private IEnumerator StopExecute()
        {
           stopReady = false;
           takeInput = false;
           
           float endTime = Time.time + stopConstants.stopLength;
           float useTimeModifier = (1 / stopConstants.stopLength) *1.5f;

           float velocityMagnitude = Rigid.velocity.magnitude;
           while (endTime > Time.time && currentFuel > 0 && Rigid.velocity.magnitude > 0.05f)
           {
               Rigid.AddForce(-Rigid.velocity * (useTimeModifier * Time.deltaTime * Rigid.velocity.magnitude), ForceMode2D.Impulse);
               yield return null;
           }
           
           Rigid.AddForce(-Rigid.velocity, ForceMode2D.Impulse);
           
           currentFuel -= velocityMagnitude * stopConstants.fuelConsumptionMultiplier;
           ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
           
           takeInput = true;
           yield return new WaitForSeconds(stopConstants.stopCooldown - stopConstants.stopLength);
           stopReady = true;
        }
        
        //TODO dash sideways

        public void DashSideways(Vector2 direction)
        {
            if (takeInput && dashSidewaysReady && currentFuel >= dashSidewaysConstants.fuelConsumption)
            {
                StartCoroutine(DashSidewaysExecute(direction));
            }
        }

        private IEnumerator DashSidewaysExecute(Vector2 direction)
        {
            takeInput = false;
            dashSidewaysReady = false;

            currentFuel -= dashSidewaysConstants.fuelConsumption;
            Rigid.AddForce(moduleBody.transform.right * (direction.x * (dashSidewaysConstants.strength)));
            yield return new WaitForSeconds(dashSidewaysConstants.length);
            Rigid.AddForce(-moduleBody.transform.right * (direction.x * (dashSidewaysConstants.strength)*0.5f));
            takeInput = true;
            yield return new WaitForSeconds(dashSidewaysConstants.cooldown - dashSidewaysConstants.length);
            dashSidewaysReady = true;
            
        }
    }
}
