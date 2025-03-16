using System;
using UnityEngine;

namespace Player.Module
{
    public class Module : MonoBehaviour
    {

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
