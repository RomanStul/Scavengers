using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum Items
    {
        TestItem
    }
    
    public Sprite image;
    public Items itemType;
}
