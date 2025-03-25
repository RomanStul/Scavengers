using System;
using UnityEngine;

namespace Player.Module.Upgrades
{
    /*##############################################################################
    //Upgrades have names from enum and are stored in array of bool that tell if upgrade is available
    //all upgrades bool -> value upgrades are set in script they take effect in
    //Example:  if(upgrades[upgrade_id]) multiplier = 1.5f;     <- set in Awake or other tart function
    //          if(ThrustInput > 0 || upgrades[upgrade_id])     <- in unction as active variable
    //##############################################################################*/
    public class Upgrades : ModuleBaseScript
    {
        //================================================================CLASSES
        public enum Ups
        {
            Reverse,
            Armor,
            Stop,
            Dash,
            DashSideWays
        }

        [Serializable]
        public class UpgradeObject
        {
            [HideInInspector]
            public string Name;
            public bool unlocked;

            public UpgradeObject(string name, bool unlocked)
            {
                this.Name = name;
                this.unlocked = unlocked;
            }
        }
        
        //================================================================EDITOR VARIABLES
        public UpgradeObject[] upgradesObject = new UpgradeObject[Enum.GetValues(typeof(Ups)).Length];
        
        //================================================================GETTER SETTER

        public void LoadUpgrades(bool[] loaded = null)
        {
            if (loaded == null)
            {
                loaded = new bool[Enum.GetValues(typeof(Ups)).Length];
            }
            
            ModuleRef.ApplyUpgrades();
        }
        
        //================================================================FUNCTIONALITY

        public void Reset()
        {
            CreateUpgradeArray();
        }

        public bool IsActive(Ups up)
        {
            return upgradesObject[(int)up].unlocked;
        }

        public void CreateUpgradeArray()
        {
            UpgradeObject[] updatedUpgrades = new UpgradeObject[Enum.GetValues(typeof(Ups)).Length];

            for (int i = 0; i < Mathf.Min(upgradesObject.Length, updatedUpgrades.Length); i++)
            {
                updatedUpgrades[i] = upgradesObject[i];
            }

            if (updatedUpgrades.Length > upgradesObject.Length)
            {
                for (int i = upgradesObject.Length; i < updatedUpgrades.Length; i++)
                {
                    updatedUpgrades[i] = new UpgradeObject(Enum.GetNames(typeof(Ups))[i], false);
                }
            }

            upgradesObject = updatedUpgrades;
        }
    }
}
