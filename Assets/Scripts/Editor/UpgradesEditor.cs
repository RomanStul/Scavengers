using Player.Module.Upgrades;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ModuleUpgrades))]
// ^ This is the script we are making a custom editor for.
    public class UpgradesEditor : UnityEditor.Editor
    {
   
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            
            ModuleUpgrades up = (ModuleUpgrades)target;
            
            if(GUILayout.Button("Create Upgrade Array")){
                up.CreateUpgradeArray();
            }
        }
    }
}
