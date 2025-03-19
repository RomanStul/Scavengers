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
            Armor
        }
        
        //================================================================EDITOR VARIABLES
        public bool[] upgrades = new bool [Enum.GetValues(typeof(Ups)).Length];
        
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public bool IsActive(Ups up)
        {
            return upgrades[(int)up];
        }

        public void CreateUpgradeArray()
        {
            bool[] updatedUpgrades = new bool[Enum.GetValues(typeof(Ups)).Length];

            for (int i = 0; i < Mathf.Min(upgrades.Length, updatedUpgrades.Length); i++)
            {
                updatedUpgrades[i] = upgrades[i];
            }

            if (updatedUpgrades.Length > upgrades.Length)
            {
                for (int i = upgrades.Length; i < updatedUpgrades.Length; i++)
                {
                    updatedUpgrades[i] = false;
                }
            }

            upgrades = updatedUpgrades;
        }
    }
}
