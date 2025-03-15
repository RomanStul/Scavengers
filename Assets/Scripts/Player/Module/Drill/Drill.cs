using System.Collections;
using UnityEngine;

namespace Player.Module.Drill
{
    public class Drill : MonoBehaviour
    {
        //================================================================
        [SerializeField] protected Player.Module.Module.DrillConstants drillConstants;
        [SerializeField] protected DrillController drillController;
        [SerializeField] protected Animator animator;
        //================================================================
        //================================================================

        private void Awake()
        {
            animator.SetFloat("ChargeTime", 1/drillConstants.chargeTime);
        }
        
        public void Use(bool start)
        {
            animator.SetBool("inUse", start);
        }

        public void UsingDrill()
        {
            //Measure distance
            //Change orientation and direction of laser
            //Damage the target
        }
    }
}
