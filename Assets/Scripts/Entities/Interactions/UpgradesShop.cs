using Player.Module;
using UnityEngine;

namespace Entities.Interactions
{
    public class UpgradesShop : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void ApplyUpgrades(Module module)
        {
            module.ApplyUpgrades();
        }
    }
}
