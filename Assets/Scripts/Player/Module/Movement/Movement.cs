using System;
using System.Collections;
using System.Timers;
using Player.UI;
using ScriptableObjects.Item;
using sounds;
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

        public class StopConstants
        {
            public float fuelConsumptionMultiplier;
            public float stopLength;
            public float stopCooldown;
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
        [SerializeField] protected StopConstants stopConstants;
        
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
            return maxFuel - currentFuel;
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
        private float maxFuel;
        private bool showedOutOfFuelLine = true;

        private bool takeInput = true,
            reverseAvailable = false,
            stopReady = false,
            appliedUpgrades = false,
            canFuelGenerate = false;

        public override void ApplyUpgrades()
        {
            maxFuel = movementVariables.MaxFuel;
            if (ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Fuel_Capacity_I)) maxFuel += 20;
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)maxFuel, UI.UIController.BarsNames.FuelBar, true);
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
            
            reverseAvailable = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Reverse);
            stopReady = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Stop);
            canFuelGenerate = ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Fuel_Generator);
            if(!appliedUpgrades) Refuel();
            appliedUpgrades = true;
        }


        private void FixedUpdate()
        {
            if (!takeInput)
            {
                return;
            }
            
            if (currentFuel <= 0 && Environment.instance.fuelConsumptionMultiplier != 0)
            {
                if (!canFuelGenerate || !GenerateFuel())
                {
                    if (showedOutOfFuelLine)
                    {
                        ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).ShowMonologHelp("Out of fuel, I should probably evacuate");
                        showedOutOfFuelLine = false;
                    }
                    VisualizeThrust();
                    return;
                }
            }
            
            if (ThrustInput > 0 || reverseAvailable && ThrustInput < 0)
            {
                currentFuel -= Mathf.Abs(ThrustInput) * movementVariables.fuelPerSecond * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
                Rigid.AddForce(moduleBody.transform.up * (ThrustInput * movementVariables.Thrust * Time.deltaTime));
                float speed = Rigid.linearVelocity.magnitude;
                //TODO make it so that input interacts with dash momentum intuitively
                if (speed > movementVariables.MaxSpeed)
                {
                    Rigid.linearVelocity = Rigid.linearVelocity.normalized * movementVariables.MaxSpeed;
                }
            }

            if (Mathf.Abs(RotationInput) > 0)
            {
                currentFuel -= Mathf.Abs(RotationInput) *movementVariables.fuelPerSecond * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
                Rigid.AddTorque(-RotationInput * movementVariables.RotationThrust * Time.deltaTime);
            }
            
            VisualizeThrust(ThrustInput, RotationInput);
            
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
        }

        public void Refuel(float amount = -1)
        {
            showedOutOfFuelLine = true;
            if (amount < 0)
            {
                currentFuel = maxFuel;
            }
            else
            {
                currentFuel += amount;
            }
            ModuleRef.GetScript<UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)currentFuel, UI.UIController.BarsNames.FuelBar);
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

           
           ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).StartCooldown(stopConstants.stopCooldown, UIController.Cooldowns.Stop);
           
           float velocityMagnitude = Rigid.linearVelocity.magnitude;
           while (endTime > Time.time && currentFuel > 0 && Rigid.linearVelocity.magnitude > 0.05f)
           {
               currentFuel -= velocityMagnitude * stopConstants.fuelConsumptionMultiplier * Time.deltaTime * Environment.instance.fuelConsumptionMultiplier;
               VisualizeStopThrust(-Rigid.linearVelocity);
               Rigid.AddForce(-Rigid.linearVelocity * (useTimeModifier * Time.deltaTime * Rigid.linearVelocity.magnitude), ForceMode2D.Impulse);
               yield return null;
           }
           
           
           Rigid.AddForce(-Rigid.linearVelocity.normalized * Mathf.Min(Rigid.linearVelocity.magnitude, currentFuel), ForceMode2D.Impulse);
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

        //Passes parameters to prevent showing thrust when input is blocked (dashing, stopping)
        private void VisualizeThrust(float thrustInputVar = 0.0f, float rotationInputVar = 0.0f)
        {
            if (thrustInputVar > 0 || thrustInputVar < 0 && reverseAvailable || rotationInputVar != 0 )
            {
                ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).PlaySound(ModuleSounds.SoundName.Thrusters, transform);
            }
            else
            {
                ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).StopSound(ModuleSounds.SoundName.Thrusters);
            }
            thrustVisuals.backMotor.SetActive(thrustInputVar > 0);
            thrustVisuals.frontMotor.SetActive(thrustInputVar < 0 && reverseAvailable);

            thrustVisuals.leftFrontMotor.SetActive(rotationInputVar > 0);
            thrustVisuals.rightRearMotor.SetActive(rotationInputVar > 0);

            thrustVisuals.rightFrontMotor.SetActive(rotationInputVar < 0);
            thrustVisuals.leftRearMotor.SetActive(rotationInputVar < 0);
        }

        private bool GenerateFuel()
        {
            Storage st = ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript);

            if (st.RemoveItem((int)ItemSO.Items.Purpura_Fungi, 1) == 1)
            {
                currentFuel += 10;
                return true;
            }

            return false;
        }
        
        
    }
}
