using Entities.Environment;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    
    [CustomEditor(typeof(OreHolder))]
    public class OreHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            
            OreHolder holder = (OreHolder)target;
            
            if(GUILayout.Button("Randomize Ids")){
                holder.SetOreIds();
            }
        }
    }
}
