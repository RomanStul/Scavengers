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
            UpgradeVisualsScript,
            UIControlsScript,
            InteractionScript,
        }
        //================================================================EDITOR VARIABLES

        //public Scripts scripts;
        public Player.Module.BaseClass[] baseScripts;
        public Rigidbody2D moveRb, rotateRb;
        public static ModuleState savedState;
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
            Debug.Log("awake");
            DontDestroyOnLoad(gameObject);
            
            for (int i = 0; i < baseScripts.Length; i++)
            {
                baseScripts[i].SetModule(this);
            }
            //Loads upgrades then calls Apply upgrades on this which calls apply upgrades on all scripts attached
            GetScript<Upgrades.Upgrades>(ScriptNames.UpgradesScript).LoadUpgrades(savedState?.upgrades);
            
            LoadFromSave();
        }

        public void ApplyUpgrades()
        {
            for (int i = 0; i < baseScripts.Length; i++)
            {
                baseScripts[i].ApplyUpgrades();
            }
        }

        public void CreateStateObject()
        {
            savedState = new ModuleState();
            savedState.health = GetScript<HealthBar>(ScriptNames.HealthBarScript).GetHealth();
            savedState.fuel = GetScript<Player.Module.Movement.Movement>(ScriptNames.MovementScript).GetFuel();
            savedState.currency = GetScript<Storage>(ScriptNames.StorageScript).PayWithCurrency();
            savedState.itemStored = GetScript<Storage>(ScriptNames.StorageScript).ItemStorage;
            savedState.upgrades = GetScript<Upgrades.Upgrades>(ScriptNames.UpgradesScript).GetUpgrades();
        }

        private void LoadFromSave()
        {
            //TODO if nul try loading from a file
            if (savedState != null)
            {
                GetScript<HealthBar>(ScriptNames.HealthBarScript).SetHealth(savedState.health);
                GetScript<Player.Module.Movement.Movement>(ScriptNames.MovementScript).SetFuel(savedState.fuel);
                GetScript<Storage>(ScriptNames.StorageScript).Currency = savedState.currency;
                GetScript<Storage>(ScriptNames.StorageScript).ItemStorage = savedState.itemStored;
            }
            else
            {
                GetScript<HealthBar>(ScriptNames.HealthBarScript).SetHealth(GetScript<HealthBar>(ScriptNames.HealthBarScript).GetMaxHealth());
            }
        }

        public void PrepareForSceneTransfer()
        {
            transform.position = Vector3.zero;
            moveRb.velocity = Vector2.zero;
            rotateRb.velocity = Vector2.zero;
        }
    }
}
