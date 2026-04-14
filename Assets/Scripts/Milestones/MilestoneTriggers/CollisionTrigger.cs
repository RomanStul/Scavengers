using System;
using story;
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
            if (action == GlobalMilestoneManager.MilestoneAction.Day)
            {
                id = StoryManager.instance.GetDayNumber();
            }
            base.Trigger();
        }
    }
}
