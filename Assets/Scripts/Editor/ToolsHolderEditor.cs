using Player.Module.Tools;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(ToolHolder))]
    public class ToolsHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            
            ToolHolder toolHolder = (ToolHolder)target;
            
            if(GUILayout.Button("Generate tool list"))
            {
                toolHolder.GenerateToolsCounts();
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}
