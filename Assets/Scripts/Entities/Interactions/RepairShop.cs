using System;
using System.Collections;
using Player.Module;
using Player.Module.Movement;
using Player.UI;
using Player.UI.RepairRefuel;
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


        public void OpenRepairShop(Module module)
        {
            UIController ui = module.GetScript<UIController>(Module.ScriptNames.UIControlsScript);
            RepairRefuel.RepairWindowParameters rwp = new RepairRefuel.RepairWindowParameters
            {
                moduleRef = module,
                refuelCost = constants.costPerFuel,
                repairCost = constants.costPerHealth
            };

            ui.PassRepairParameters(rwp);
            ui.OpenWindow(UIController.WindowType.Repair);
            Debug.Log("should open");
            
        }
    }
}
