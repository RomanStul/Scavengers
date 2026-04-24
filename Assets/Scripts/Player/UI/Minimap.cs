using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Environment = Entities.Environment.Environment;

namespace Player.UI
{
    public class Minimap : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private Image minimapTexture;
        [SerializeField] private Texture overlay;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private Module.Module moduleRef;

        private Vector2 initialPosition;
        private Vector2 initialWorldsPos;
        
        private Material instancedMaterial;
        
        public void SetupMinimap(Module.Module m)
        { 
            moduleRef = m;
            initialPosition = Environment.instance.GetMinimapCenter();
            initialWorldsPos = Environment.instance.GetModuleSpawnLocation();
           
            instancedMaterial = Instantiate(minimapTexture.material);
            minimapTexture.material = instancedMaterial;
            minimapTexture.material.SetTexture("_mask", Environment.instance.GetMinimap().texture);
            minimapTexture.material.SetTexture("_overlay", overlay);
            minimapTexture.material.SetTexture("_colorTexture", Environment.instance.GetMinimap().texture);
            minimapTexture.material.SetVector("_overlayScale", new Vector4(1, ((float)Environment.instance.GetMinimap().texture.height * 8)/((float)overlay.height * 3), 0, 0));
           
            minimapTexture.rectTransform.sizeDelta = new Vector2(Environment.instance.GetMinimap().texture.width, Environment.instance.GetMinimap().texture.height) * 8; 
            ((RectTransform)minimapTexture.transform).anchoredPosition = -initialPosition - ((Convertor.Vec3ToVec2(moduleRef.transform.position) - initialWorldsPos) / 1.25f) * 8f;
        }
        
        private void Update()
        {
            //position of texture = initial pixel offset + offset in world / convertor to grid * multiply to pixel offset
            ((RectTransform)minimapTexture.transform).anchoredPosition = -initialPosition - Convertor.RoundVector2((Convertor.Vec3ToVec2(moduleRef.transform.position) - initialWorldsPos) / 1.25f) * 8f;
        }
    }
}
