using System;
using System.Collections;
using System.Timers;
using Player.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Environment = Entities.Environment.Environment;

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

        public class MoveSidewaysConstants
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
        [SerializeField] protected MoveSidewaysConstants moveSidewaysConstants;
        
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

        protected Rigidbody2D Rigid;

        private bool takeInput = true,
            dashReady = false,
            reverseAvailable = false,
            stopReady = false,
            moveSidewaysReady = false,
            moveSidewaysEneabled = false;
        
        private float sidewaysInput = 0f;

        public override void ApplyUpgrades()
        {
            //TODO space for fuel space increase
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)movementVariables.MaxFuel, UI.UIController.BarsNames.FuelBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
            dashReady = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Dash);
            reverseAvailable = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Reverse);
            stopReady = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Stop);
            moveSidewaysReady = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Sideways_Thrust);
            
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
                VisualizeThrust();
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
                Rigid.AddTorque(-RotationInput * movementVariables.RotationThrust * Time.deltaTime);
            }

            if (Mathf.Abs(sidewaysInput) > 0 && moveSidewaysEneabled)
            {
                currentFuel -= Mathf.Abs(sidewaysInput) * moveSidewaysConstants.fuelConsumption * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
                Rigid.AddForce(moduleBody.transform.right * (sidewaysInput * moveSidewaysConstants.strength * Time.deltaTime));
            }
            
            VisualizeThrust(ThrustInput, RotationInput, sidewaysInput);
            
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
            VisualizeThrust(1.0f);
            
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
               VisualizeStopThrust(-Rigid.velocity);
               Rigid.AddForce(-Rigid.velocity * (useTimeModifier * Time.deltaTime * Rigid.velocity.magnitude), ForceMode2D.Impulse);
               yield return null;
           }
           
           
           Rigid.AddForce(-Rigid.velocity.normalized * Mathf.Min(Rigid.velocity.magnitude, currentFuel), ForceMode2D.Impulse);
           VisualizeStopThrust(new Vector2(0.0f, 0.0f));
           
           ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
           
           takeInput = true;
           yield return new WaitForSeconds(stopConstants.stopCooldown - stopConstants.stopLength);
           stopReady = true;
        }

        private void VisualizeStopThrust(Vector2 stopDirection)
        {
            stopDirection.Normalize();
            stopDirection.x = (stopDirection.x - stopDirection.x * 0.5f) * 2;
            stopDirection.y = (stopDirection.y - stopDirection.y * 0.5f) * 2;
            
            Vector2 upThrust = stopDirection * moduleBody.transform.up;
            float upSum = upThrust.x + upThrust.y;
            Vector2 rightThrust = stopDirection * moduleBody.transform.right;
            float rightSum = rightThrust.x + rightThrust.y;

            thrustVisuals.backMotor.SetActive(upSum > 0.4f);
            thrustVisuals.frontMotor.SetActive(upSum < -0.4 && reverseAvailable);

            thrustVisuals.leftFrontMotor.SetActive(rightSum > 0.4f);
            thrustVisuals.leftRearMotor.SetActive(rightSum > 0.4f);
            
            thrustVisuals.rightFrontMotor.SetActive(rightSum < -0.4f);
            thrustVisuals.rightRearMotor.SetActive(rightSum < -0.4f);
        }
        
        
        public void MoveSideways(Vector2 direction)
        {
            if (moveSidewaysReady)
            {
                StartCoroutine(MoveSidewaysExecute());
            }

            sidewaysInput = direction.x;
        }

        private IEnumerator MoveSidewaysExecute()
        {
            moveSidewaysReady = false;
            moveSidewaysEneabled = true;
            
            //TODO start reverse visual cooldown
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).StartDuration(moveSidewaysConstants.length);
            yield return new WaitForSeconds(moveSidewaysConstants.length);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).StartCooldown(moveSidewaysConstants.cooldown, UIController.Cooldowns.SideDash);
            moveSidewaysEneabled = false;
            
            yield return new WaitForSeconds(moveSidewaysConstants.cooldown - moveSidewaysConstants.length);
            moveSidewaysReady = true;
            
        }

        //Passes parameters to prevent showing thrust when input is blocked (dashing, stopping)
        private void VisualizeThrust(float thrustInputVar = 0.0f, float rotationInputVar = 0.0f, float sidewaysInputVar = 0.0f)
        {
            thrustVisuals.backMotor.SetActive(thrustInputVar > 0);
            thrustVisuals.frontMotor.SetActive(thrustInputVar < 0 && reverseAvailable);

            thrustVisuals.leftFrontMotor.SetActive(rotationInputVar > 0 || sidewaysInputVar > 0 && moveSidewaysEneabled);
            thrustVisuals.rightRearMotor.SetActive(rotationInputVar > 0 || sidewaysInputVar < 0 && moveSidewaysEneabled);

            thrustVisuals.rightFrontMotor.SetActive(rotationInputVar < 0 || sidewaysInputVar < 0 && moveSidewaysEneabled);
            thrustVisuals.leftRearMotor.SetActive(rotationInputVar < 0 || sidewaysInputVar > 0 && moveSidewaysEneabled);
        }
        
        
    }
}
