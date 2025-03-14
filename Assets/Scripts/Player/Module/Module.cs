using System;
using UnityEngine;

namespace Player.Module
{
    public class Module : MonoBehaviour
    {
        public Player.Module.Input inputScript;
        public Player.Module.Movement.Movement movementScript;

        private void Awake()
        {
            inputScript.SetModule(this);
            movementScript.SetModule(this);
        }
    }
}
