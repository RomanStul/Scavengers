using UnityEngine;
using Player.Module.Upgrades;
using Player.Module;

namespace Player.UI
{
    public class UIController : Module.ModuleBaseScript
    {
        public enum BarsNames
        {
            HealthBar,
            FuelBar,
            StorageBar
        }

        public enum Cooldowns
        {
            Dash,
            Stop,
            SideDash
        }
        //================================================================CLASSES

        //================================================================EDITOR VARIABLES
        [SerializeField] private Bar HealthBar, FuelBar, StorageBar;
        [SerializeField] private Cooldown Dash, Stop, SideDash;
        //================================================================GETTER SETTER

        //================================================================FUNCTIONALITY

        public override void ApplyUpgrades()
        {
            Dash.transform.gameObject.SetActive(
                    ModuleRef.GetScript<Upgrades>(Module.Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Ups.Dash)
                );
            Stop.transform.gameObject.SetActive(
                ModuleRef.GetScript<Upgrades>(Module.Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Ups.Stop)
            );
        }

    public void SetBar(int value, BarsNames barName, bool isMax = false)
        {
            Bar targetBar = null;

            switch (barName)
            {
                case BarsNames.HealthBar:
                    targetBar = HealthBar;
                    break;
                case BarsNames.FuelBar:
                    targetBar = FuelBar;
                    break;
                case BarsNames.StorageBar:
                    targetBar = StorageBar;
                    break;
                default:
                    return;
            }

            if (isMax)
            {
                targetBar.SetMaxValue(value);
            }
            else
            {
                targetBar.SetValue(value);
            }
        }

        public void StartCooldown(float time, Cooldowns cooldownName)
        {
            Cooldown targetCooldown = null;

            switch (cooldownName)
            {
                case Cooldowns.Dash:
                    targetCooldown = Dash;
                    break;
                case Cooldowns.Stop:
                    targetCooldown = Stop;
                    break;
                case Cooldowns.SideDash:
                    targetCooldown = SideDash;
                    break;
                default:
                    return;
            }
            
            targetCooldown.StartCooldown(time);
        }
    }
}
