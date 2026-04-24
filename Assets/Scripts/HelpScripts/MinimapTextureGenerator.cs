using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Environment = Entities.Environment.Environment;

namespace HelpScripts
{
    public class MinimapTextureGenerator : MonoBehaviour
    {
        //================================================================CLASSES

        [Serializable]
        private class TileColor
        {
            public Color[] colors;
            public TileBase[] tiles;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private TileColor[] tileColors;
        [SerializeField] private Tilemap[] tilemaps;


        [SerializeField] private Vector2Int bottomLeftCorner, topRightCorner, center;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void GenerateTexture()
        {
            Texture2D texture = new Texture2D(topRightCorner.x - bottomLeftCorner.x, (topRightCorner.y - bottomLeftCorner.y), TextureFormat.ARGB32, false);
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, Color.black);
                }
            }
            DrawTilePixels(texture);
            
            File.WriteAllBytes("Assets/Textures/MinimapTextures/" + SceneManager.GetActiveScene().name + ".png", texture.EncodeToPNG());
            Vector2Int textureOffset = (center - bottomLeftCorner)*8;
            Debug.Log("minimapFinished " + textureOffset);
        }

        private void DrawTilePixels(Texture2D texture)
        {
            foreach (Tilemap t in tilemaps)
            {
                for (int x = bottomLeftCorner.x; x <= topRightCorner.x; x++)
                {
                    for (int y = bottomLeftCorner.y; y <= topRightCorner.y; y++)   
                    {
                        if (t.GetTile(new Vector3Int(x, y, 0)) is { } tile)
                        {
                            Color colorOfPixel = FindColorForTile(tile);
                            if (colorOfPixel != Color.clear)
                            {
                                if (colorOfPixel.a < 1)
                                {
                                    Color final = texture.GetPixel(x + Mathf.Abs(bottomLeftCorner.x), y + Mathf.Abs(bottomLeftCorner.y)) * (1 - colorOfPixel.a) + (colorOfPixel * colorOfPixel.a);
                                    final.a = 1;
                                    texture.SetPixel(x+Mathf.Abs(bottomLeftCorner.x),y+Mathf.Abs(bottomLeftCorner.y), final);
                                }
                                else
                                {
                                    texture.SetPixel(x+Mathf.Abs(bottomLeftCorner.x),y+Mathf.Abs(bottomLeftCorner.y), colorOfPixel);
                                }
                            }


                        }
                        
                    }
                }
            }
        }

        private Color FindColorForTile(TileBase tile)
        {
            for (int i = 0; i < tileColors.Length; i++)
            {
                for (int j = 0; j < tileColors[i].tiles.Length; j++)
                {
                    if (tileColors[i].tiles[j] == tile)
                    {
                        return tileColors[i].colors[UnityEngine.Random.Range(0, tileColors[i].colors.Length)];
                    }
                }
            }
            return Color.clear;
        }

    }
}
