using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Entities
{
    public class SpecifiedItemDropper : ItemDropper
    {
        //================================================================CLASSES

        //================================================================EDITOR VARIABLES
        [SerializeField] private SpriteRenderer itemShow;

        [SerializeField] private ToolSO[] tools;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public override void Awake()
        {
            base.Awake();
            if (drops.Length + tools.Length > 0)
            {
                if (drops.Length + tools.Length > 1)
                {
                    StartCoroutine(CycleDrops());
                }
                else
                {
                    itemShow.sprite = drops.Length > 0 ? drops[0].item.image : tools[0].droppedIcon;
                }
                
            }
        }

        private IEnumerator CycleDrops()
        {
            while (true)
            {
                for (int i = 0; i < tools.Length; i++)
                {
                    itemShow.sprite = tools[i % tools.Length].droppedIcon;
                    yield return new WaitForSeconds(1.5f);
                }

                for (int i = 0; i < drops.Length; i++)
                {
                    itemShow.sprite = drops[i].item.image;
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }

        public override void DropItems()
        {
            base.DropItems();

            foreach (var tool in tools)
            {
                Item sp = SpawnItemAtRandomOffset(transform.position);
                sp.SetToolData(tool);
            }
        }
    }
}
