using System;
using Player.Module;
using Player.Module.Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Interactions
{
    public class RepairShop : MonoBehaviour
    {
        
        //================================================================CLASSES

        [Serializable]
        private class RepairShopConstants
        {
            public float costPerHealth;
            public float costPerFuel;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private RepairShopConstants constants;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY


        public void Repair(Module module)
        {
            Player.Module.HealthBar healthBar = module.GetScript<Player.Module.HealthBar>(Module.ScriptNames.HealthBarScript);
            float missingHealth = healthBar.GetMissingHealth();
            
            healthBar.HealHealth(GetAmountToWork(module, missingHealth, constants.costPerHealth));
        }

        public void Refuel(Module module)
        {
            Movement movement = module.GetScript<Movement>(Module.ScriptNames.MovementScript);
            float missing = movement.GetMissingFuel();
            
            movement.Refuel(GetAmountToWork(module, missing, constants.costPerFuel));
        }

        private float GetAmountToWork(Module module, float toFull, float perUnit)
        {
            if (toFull == 0)
            {
                return 0;
            }
            float cost = toFull * perUnit;
            Storage storage = module.GetScript<Storage>(Module.ScriptNames.StorageScript);
            float paid = storage.GetCurrency((int)cost);
            
            float toReturn = toFull * (paid / cost);
            return toReturn;
        }
    }
}
