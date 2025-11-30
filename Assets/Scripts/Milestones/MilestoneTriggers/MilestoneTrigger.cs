using UnityEngine;

namespace Milestones.MilestoneTriggers
{
    public class MilestoneTrigger : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        [SerializeField] private int id;
        [SerializeField] private bool disableAfterTrigger = true;
        //================================================================

        protected void Trigger()
        {
            SceneMilestoneManager.currentInstance.CompletedMilestone(new SceneMilestoneManager.Milestone(SceneMilestoneManager.MilestoneAction.Entered, id));
            if (disableAfterTrigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
