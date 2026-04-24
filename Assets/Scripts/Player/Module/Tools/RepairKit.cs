using ScriptableObjects.Tools;
using story;
using UnityEngine;

namespace Player.Module.Tools
{
    public class RepairKit : ModuleTool
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolSO repairKitRef;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public override void Use(Module m)
        {
            m.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript).HealHealth(repairKitRef.price / (StoryManager.instance.GetRepairMult() * 0.25f));
        }
    }
}
