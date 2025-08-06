using UnityEngine;


namespace Player.Module
{
    public class ModuleState
    {

        
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        public float health, fuel;
        public int[] itemStored;
        public int currency;
        public bool[] upgrades;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void SaveUpgrades(Upgrades.ModuleUpgrades.UpgradeObject[] upgradesToSave)
        {
            upgrades = new bool[upgradesToSave.Length];
            for (int i = 0; i < upgradesToSave.Length; i++)
            {
                upgrades[i] = upgradesToSave[i].unlocked;
            }
        }
    }
    
    //TODO serialize and deserialize into file
}
