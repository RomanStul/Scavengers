using System;
using UnityEngine;

namespace Player.Module
{
    public class Module : MonoBehaviour
    {
        
        //================================================================CLASSES
        [Serializable]

        public class Scripts
        {
            public Player.Module.Input inputScript;
            public Player.Module.Movement.Movement movementScript;
            public Player.Module.Drill.DrillController drillScript;
            public Player.Module.CollisionHandler collisionScript;
            public Player.Module.Storage storageScript;
            public Player.Module.HealthBar healthBarScript;
        }
        //================================================================EDITOR VARIABLES

        public Scripts scripts;
        public Rigidbody2D moveRb, rotateRb;
        //================================================================GETTER SETTER
        public Rigidbody2D GetMoveRb()
        {
            return moveRb;
        }

        public Rigidbody2D GetRotateRb()
        {
            return rotateRb;
        }
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            scripts.inputScript.SetModule(this);
            scripts.movementScript.SetModule(this);
            scripts.drillScript.SetModule(this);
            scripts.collisionScript.SetModule(this);
            scripts.storageScript.SetModule(this);
        }
    }
}
