using System;
using Entities;
using Player.Module.Upgrades;
using Player.UI;
using UnityEngine;

namespace Player.Module.Drill
{
    public class DrillController : ModuleBaseScript
    {
        //================================================================CLASSES
        [Serializable]
        public class DrillControllerConstants
        {
            public float RotationSpeed;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] protected DrillControllerConstants drillControllerConstants;
        [SerializeField] protected GameObject drillGameObject;
        [SerializeField] protected Player.Module.Drill.Drill drill;
        //================================================================GETTER SETTER
        public Vector2 DrillTargetPosition { get; set; } = new Vector2(0,0);
        //================================================================FUNCTIONALITY
        private bool canUseHarpoon;

        public override void ApplyUpgrades()
        {
            canUseHarpoon = ModuleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(ModuleUpgrades.Ups.Harpoon);
            if(canUseHarpoon) drill.SetUpHarpoon(ModuleRef.GetMoveRb());
        }

        private void FixedUpdate()
        {
            //TODO make follow body of module
            Vector3 targetPosition = Convertor.Vec2ToVec3((DrillTargetPosition - new Vector2(Screen.width / 2, Screen.height / 2)));
            //targetPosition += transform.position;
            Convertor.Lerp2D(targetPosition, drillGameObject.transform, drillControllerConstants.RotationSpeed);
        }

        public void UseDrill(bool isDrilling)
        {
            drill.Use(isDrilling);
        }

        public void PlayDrillSound(bool play, ModuleSounds.SoundName sound)
        {
            if (play)
            {
                ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).PlaySound(sound, transform);
            }
            else
            {
                ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).StopSound(sound);
            }
        }

        public void UseHarpoon(bool started)
        {
            if(!canUseHarpoon) return;

            if(started)
                ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar(0, UIController.BarsNames.Harpoon);
            drill.UseHarpoon(started);
        }

        public void HarpoonHasRecalled(Item item)
        {
            if (item != null)
            {
                ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript).PickUpItem(item, 1);
            }
            
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar(1, UIController.BarsNames.Harpoon);

        }
    }
}
