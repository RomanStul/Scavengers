using UnityEngine;

namespace Milestones.MilestoneTriggers
{
    public class MilestoneTrigger : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        [SerializeField] protected int id;
        [SerializeField] protected bool disableAfterTrigger = true;
        [SerializeField] protected GlobalMilestoneManager.MilestoneAction action = GlobalMilestoneManager.MilestoneAction.Entered;
        //================================================================

        protected void Trigger()
        {
            SceneMilestoneManager.currentInstance.CompletedMilestone(new GlobalMilestoneManager.Milestone(action, id));
            if (disableAfterTrigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
