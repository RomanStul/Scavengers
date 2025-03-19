using System;
using UnityEngine;
using UnityEngine.Serialization;

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
            public Player.Module.Upgrades.Upgrades upgradesScript;
            public Player.Module.Upgrades.UpgradeVisuals upgradeVisualsScript;
        }

        public enum ScriptNames
        {
            InputScript,
            MovementScript,
            DrillScript,
            CollisionScript,
            StorageScript,
            HealthBarScript,
            UpgradesScript,
            UpgradeVisualsScript
        }
        //================================================================EDITOR VARIABLES

        //public Scripts scripts;
        public Player.Module.BaseClass[] baseScripts;
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

        public T GetScript<T>(ScriptNames scriptName) where T : BaseClass
        {
            return (T)baseScripts[(int)scriptName];
        }
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            for (int i = 0; i < baseScripts.Length; i++)
            {
                baseScripts[i].SetModule(this);
            }
        }

        public void ApplyUpgrades()
        {
            for (int i = 0; i < baseScripts.Length; i++)
            {
                baseScripts[i].ApplyUpgrades();
            }
        }
    }
}
