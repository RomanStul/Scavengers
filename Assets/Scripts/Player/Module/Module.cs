using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module
{
    public class Module : MonoBehaviour
    {
        
        //================================================================CLASSES

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
            //TODO make so that order of script either doesn't matter or is fixed to correct one
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
