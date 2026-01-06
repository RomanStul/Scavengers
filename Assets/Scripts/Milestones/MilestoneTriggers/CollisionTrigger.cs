using System;
using UnityEngine;

namespace Milestones.MilestoneTriggers
{
    public class CollisionTrigger : MilestoneTrigger
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        private void OnTriggerEnter2D(Collider2D other)
        {
            base.Trigger();
        }
    }
}
