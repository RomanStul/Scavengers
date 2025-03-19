using Player.Module.Upgrades;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Upgrades))]
// ^ This is the script we are making a custom editor for.
    public class UpgradesEditor : UnityEditor.Editor
    {
   
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            
            Upgrades up = (Upgrades)target;
            
            if(GUILayout.Button("Create Upgrade Array")){
                up.CreateUpgradeArray();
            }
        }
    }
}
