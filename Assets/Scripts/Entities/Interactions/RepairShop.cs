using System;
using Player.Module;
using Player.Module.Movement;
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
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY


        public static void Repair(Module module)
        {
            //TODO check against currency and heal for amount that can player afford
            Player.Module.HealthBar healthBar = module.GetScript<Player.Module.HealthBar>(Module.ScriptNames.HealthBarScript);
            healthBar.HealHealth();
        }

        public static void Refuel(Module module)
        {
            //TODO check against currency and refuel for amount that can player afford
            Movement movement = module.GetScript<Movement>(Module.ScriptNames.MovementScript);
            movement.Refuel();
        }
    }
}
