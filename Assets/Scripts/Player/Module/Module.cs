using System;
using UnityEngine;

namespace Player.Module
{
    public class Module : MonoBehaviour
    {
        [Serializable]
        public class MovementConstants
        {
            public float Thrust;
            public float RotationThrust;
            public float MaxSpeed;
        }

        [Serializable]
        public class DrillControllerConstants
        {
            public float RotationSpeed;
        }

        [Serializable]
        public class DrillConstants
        {
            public float range;
            public float chargeTime;
        }

        [Serializable]

        public class Scripts
        {
            public Player.Module.Input inputScript;
            public Player.Module.Movement.Movement movementScript;
            public Player.Module.Drill.DrillController drillScript;
        }
        //================================================================

        public Scripts scripts;
        //================================================================
        //================================================================

        private void Awake()
        {
            scripts.inputScript.SetModule(this);
            scripts.movementScript.SetModule(this);
            scripts.drillScript.SetModule(this);
        }
    }
}
