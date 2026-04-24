using System;
using Player.Module;
using Player.Module.Tools;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class ShipWreck : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolSO key;

        [SerializeField] private Sprite keySprite;
        [SerializeField] private Transform keySpawnPoint;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            if (Environment.instance.GetModuleRef().GetScript<ToolHolder>(Module.ScriptNames.ToolScript).GetAmountOfToolType(ToolSO.ToolType.Blue_Pad) <= 0)
            {
                Item dropped = ItemDropper.SpawnItemAtRandomOffset(keySpawnPoint.position);
                dropped.SetToolData(key);
                dropped.SetSprite(keySprite);
                dropped.IncreaseDetectTriggerSize(3f);
            }
        }
    }
}
