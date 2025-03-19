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
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MovementConstants movementVariables;
        
        [SerializeField] protected GameObject moduleBody;
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
        
        private void Update()
        {
            //TODO add fuel consumption functionality
            //TODO change if so that it doesn't go through with 0 as input
            
            if (ThrustInput > 0 || ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Reverse))
            {
                Rigid.AddForce(moduleBody.transform.up * (ThrustInput * movementVariables.Thrust));
                float speed = Rigid.velocity.magnitude;
                if (speed > movementVariables.MaxSpeed)
                {
                    Rigid.velocity = Rigid.velocity.normalized * movementVariables.MaxSpeed;
                }
            }

            if (Mathf.Abs(RotationInput) > 0)
            {
                RotationRigid.AddTorque(-RotationInput * movementVariables.RotationThrust);
            }
        }
    }
}
