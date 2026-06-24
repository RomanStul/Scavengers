using System;
using UnityEngine;

namespace ScriptableObjects.MapTextures
{
    [Serializable]
    public class ColorRow
    {
        public Color[] row;
    }
    
    [CreateAssetMenu(fileName = "MapTexture", menuName = "ScriptableObjects/MapTextureSO")]
    public class MapTextureSO : ScriptableObject
    {
        public Sprite[] texture;
        public ColorRow[] colorRows;
    }
}
