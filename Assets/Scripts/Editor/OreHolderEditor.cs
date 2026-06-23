using Entities.Environment;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    
    [CustomEditor(typeof(SavedObjectHolder))]
    public class OreHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            
            SavedObjectHolder holder = (SavedObjectHolder)target;
            
            if(GUILayout.Button("Randomize Ids")){
                holder.SetId();
            }
        }
    }
}
