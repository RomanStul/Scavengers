using UnityEngine;

namespace ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemDB_SO", order = 1)]
    public class ItemDBSO : ScriptableObject
    {
        public ItemSO[] items;
    }
}
