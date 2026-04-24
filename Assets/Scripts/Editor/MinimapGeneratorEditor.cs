using HelpScripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MinimapTextureGenerator))]
    public class MinimapGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            
            MinimapTextureGenerator gen = (MinimapTextureGenerator)target;
            
            if(GUILayout.Button("Create Minimap")){
                gen.GenerateTexture();
            }
        }
    }
}
