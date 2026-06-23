using System;
using Player.Module;
using Player.Module.Tools;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class KeySpawner : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolSO key;

        [SerializeField] private Sprite keySprite;
        [SerializeField] private Transform keySpawnPoint;

        [SerializeField] private Barricade[] doorsToOpen;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private void Awake()
        {
            if (Environment.instance.GetModuleRef().GetScript<ToolHolder>(Module.ScriptNames.ToolScript).GetAmountOfToolType(ToolSO.ToolType.Blue_Pad) <= 0 && NeedKey())
            {
                Item dropped = ItemDropper.SpawnItemAtRandomOffset(keySpawnPoint.position);
                dropped.SetToolData(key);
                dropped.IncreaseDetectTriggerSize(3f);
            }
        }

        private bool NeedKey()
        {
            foreach (Barricade b in doorsToOpen)
            {
                if (b.GetActivation())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
