using System;
using System.Collections;
using Player.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Environment = Entities.Enviroment.Environment;

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

        [Serializable]
        
        public class ThrustVisuals
        {
            public GameObject backMotor;
            public GameObject frontMotor;
            public GameObject leftFrontMotor;
            public GameObject rightFrontMotor;
            public GameObject leftRearMotor;
            public GameObject rightRearMotor;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MovementConstants movementVariables;
        [SerializeField] protected DashConstants dashConstants;
        [SerializeField] protected StopConstants stopConstants;
        [SerializeField] protected DashSidewaysConstants dashSidewaysConstants;
        
        [SerializeField] protected ThrustVisuals thrustVisuals;
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

        public float GetMissingFuel()
        {
            return movementVariables.MaxFuel - currentFuel;
        }

        public float GetFuel()
        {
            return currentFuel;
        }

        public void SetFuel(float fuel)
        {
            currentFuel = fuel;
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
                VisualizeThrust(0.0f, 0.0f);
                return;
            }
            
            if (ThrustInput > 0 || reverseAvailable && ThrustInput < 0)
            {
                currentFuel -= Mathf.Abs(ThrustInput) * movementVariables.fuelPerSecond * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
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
                currentFuel -= Mathf.Abs(RotationInput) *movementVariables.fuelPerSecond * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
                RotationRigid.AddTorque(-RotationInput * movementVariables.RotationThrust * Time.deltaTime);
            }
            
            VisualizeThrust(ThrustInput, RotationInput);
            
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
            VisualizeThrust(1.0f, 0.0f);
            
            dashReady = false;
            takeInput = false;

            currentFuel -= dashConstants.DashFuelConsumption * Environment.instance.fuelConsumptionMultiplier;
            Rigid.AddForce(moduleBody.transform.up * (dashConstants.DashPower));
            
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).StartCooldown(dashConstants.DashCooldown, UIController.Cooldowns.Dash);
            
            yield return new WaitForSeconds(dashConstants.DashLength);
            takeInput = true;
            yield return new WaitForSeconds(dashConstants.DashCooldown - dashConstants.DashLength);
            dashReady = true;
        }

        public void Stop()
        {
            if (currentFuel >= dashConstants.DashFuelConsumption && stopReady && takeInput)
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

           
           ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).StartCooldown(stopConstants.stopCooldown, UIController.Cooldowns.Stop);
           
           float velocityMagnitude = Rigid.velocity.magnitude;
           while (endTime > Time.time && currentFuel > 0 && Rigid.velocity.magnitude > 0.05f)
           {
               currentFuel -= velocityMagnitude * stopConstants.fuelConsumptionMultiplier * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
               Rigid.AddForce(-Rigid.velocity * (useTimeModifier * Time.deltaTime * Rigid.velocity.magnitude), ForceMode2D.Impulse);
               yield return null;
           }
           
           
           Rigid.AddForce(-Rigid.velocity.normalized * Mathf.Min(Rigid.velocity.magnitude, currentFuel), ForceMode2D.Impulse);
           
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
            //TODO try removing velocity and adding it after execute concludes
            takeInput = false;
            dashSidewaysReady = false;

            currentFuel -= dashSidewaysConstants.fuelConsumption * Environment.instance.fuelConsumptionMultiplier;
            Rigid.AddForce(moduleBody.transform.right * (direction.x * (dashSidewaysConstants.strength)));
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).StartCooldown(dashSidewaysConstants.cooldown, UIController.Cooldowns.SideDash);
            yield return new WaitForSeconds(dashSidewaysConstants.length);
            takeInput = true;
            yield return new WaitForSeconds(dashSidewaysConstants.cooldown - dashSidewaysConstants.length);
            dashSidewaysReady = true;
            
        }


        private void VisualizeThrust(float thrustInput, float rotationInput)
        {
            thrustVisuals.backMotor.SetActive(thrustInput > 0);
            thrustVisuals.frontMotor.SetActive(thrustInput < 0 && reverseAvailable);
            
            thrustVisuals.leftFrontMotor.SetActive(rotationInput > 0);
            thrustVisuals.rightRearMotor.SetActive(rotationInput > 0);

            thrustVisuals.rightFrontMotor.SetActive(rotationInput < 0);
            thrustVisuals.leftRearMotor.SetActive(rotationInput < 0);
        }
    }
}
