using System;
using Player.Module;
using Player.Module.Tools;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Entities.Environment.Traps_and_puzzles
{
    public class KeySpawner : ItemDropper
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] private ToolSO key;

        [SerializeField] private Sprite keySprite;
        [SerializeField] private Transform keySpawnPoint;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public override void Awake()
        {
            if (!DestructionManager.instance.CheckForOre(Id))
            {
                Item dropped = ItemDropper.SpawnItemAtRandomOffset(keySpawnPoint.position);
                dropped.SetToolData(key);
                dropped.IncreaseDetectTriggerSize(3f);
                DestructionManager.instance.AddOre(Id);
            }
            

        }
    }
}
