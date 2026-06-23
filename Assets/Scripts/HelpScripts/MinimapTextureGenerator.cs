using System;
using System.IO;
using ScriptableObjects.MapTextures;
using Unity.VisualScripting;
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
        [SerializeField] private MapTextureSO[] objectTextures;
        [SerializeField] private Transform[] objectParents;

        [SerializeField] private Vector2Int bottomLeftCorner, topRightCorner, center;
        //================================================================GETTER SETTER

        public Texture2D GetObjectTextures()
        {
            return GenerateObjectTexture();
        }
        //================================================================FUNCTIONALITY
        
        private Texture2D objectTexture;

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
        }

        public Texture2D GenerateObjectTexture()
        {
            Texture2D texture = new Texture2D(topRightCorner.x - bottomLeftCorner.x, (topRightCorner.y - bottomLeftCorner.y), TextureFormat.ARGB32, false);
            
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }
            
            foreach (Transform objectParent in objectParents)
            {
                GoOverObjects(texture, objectParent);
            }
            
            //File.WriteAllBytes("Assets/Textures/MinimapTextures/" + SceneManager.GetActiveScene().name + "Objects.png", texture.EncodeToPNG());
            texture.Apply();
            texture.filterMode = FilterMode.Point;

            return texture;
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

        private void GoOverObjects(Texture2D texture, Transform parent)
        {
            Vector2 initialWorldPos = transform.GetComponent<Environment>().GetModuleSpawnLocation();
            
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (!child.gameObject.activeSelf)
                {
                    continue;
                }
                SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    if (renderer.sprite != null)
                    {
                        MapTextureSO so = FindTextureSO(renderer.sprite.name);
                        if (so is not null)
                        {
                            DrawObjects(so, child.position, texture, initialWorldPos, child.rotation.eulerAngles.z);
                        }
                    }
                }
                GoOverObjects(texture, child);
            }
        }

        private void DrawObjects(MapTextureSO textureSO, Vector3 position, Texture2D texture, Vector2 worldInitial, float rotationZ)
        {
            
            if (textureSO.colorRows.Length == 1 && textureSO.colorRows[0].row.Length == 1)
            {
                Vector2Int mapCoords = CalculateObjectMapCoords(position, worldInitial);
                texture.SetPixel(mapCoords.x, mapCoords.y, textureSO.colorRows[0].row[0].WithAlpha(1));
                return;
            }
            
            int rows = textureSO.colorRows.Length;
            int columns = textureSO.colorRows[0].row.Length;

            if (rotationZ is < 45 or > 135 and < 225 or > 315)
            {
                if (rows % 2 == 0) position.y += 0.625f;
                if(columns % 2 == 0) position.x += 0.625f;
                position.y -= rows / 2 * 1.25f;
                position.x -= columns / 2 * 1.25f;
                
                Vector2Int mapCoords = CalculateObjectMapCoords(position, worldInitial);

                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        texture.SetPixel(mapCoords.x + column, mapCoords.y + row, textureSO.colorRows[row].row[column].WithAlpha(1));
                    }
                }
            }
            else
            {
                if (rows % 2 == 0) position.x += 0.625f;
                if(columns % 2 == 0) position.y += 0.625f;
                position.x -= rows / 2 * 1.25f;
                position.y -= columns / 2 * 1.25f;
                
                Vector2Int mapCoords = CalculateObjectMapCoords(position, worldInitial);

                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        texture.SetPixel(mapCoords.x + row, mapCoords.y + column, textureSO.colorRows[row].row[column].WithAlpha(1));
                    }
                }
            }

        }

        private Vector2Int CalculateObjectMapCoords(Vector3 position, Vector2 worldInitial)
        {
            Vector2 tileCoords = center +  Convertor.RoundVector2((Convertor.Vec3ToVec2(position) - worldInitial) / 1.25f);
            int textPosX = Mathf.Abs((int)tileCoords.x - bottomLeftCorner.x);
            int textPosY = Mathf.Abs((int)tileCoords.y - bottomLeftCorner.y);
            return new Vector2Int(textPosX, textPosY);
        }

        private MapTextureSO FindTextureSO(string textureName)
        {
            for (int i = 0; i < objectTextures.Length; i++)
            {
                for (int j = 0; j < objectTextures[i].texture.Length; j++)
                {
                    if (objectTextures[i].texture[j].name == textureName)
                    {
                        return objectTextures[i];
                    }
                }
            }

            return null;
        }

    }
}
