using System;
using System.Collections.Generic;
using Player.Module;
using Player.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Milestones
{
    public class SceneMilestoneManager : MonoBehaviour
    {
        //================================================================CLASSES

        public enum MilestoneAction
        {
            Entered,
            PickedUp,
            Destroyed
        }
        
        [Serializable]
        public class Milestone
        {
            public Milestone(MilestoneAction a, int id)
            {
                this.action = a;
                this.originID = id;
            }

            public bool Equals(Milestone other)
            {
                return action.Equals(other.action) && originID == other.originID;
            }

            public MilestoneAction action;
            public int originID;
        }

        [Serializable]
        public class MilestoneEvent
        {
            public Milestone milestone;
            public UnityEvent completionEvent;
            public bool completed = false;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private List<MilestoneEvent> milestones;
        //================================================================GETTER SETTER
        public void SetModule(Module m)
        {
            moduleRef = m;
        }
        //================================================================FUNCTIONALITY
        private Module moduleRef;
        public static SceneMilestoneManager currentInstance;


        private void Awake()
        {
            currentInstance = this;
        }


        public void CompletedMilestone(Milestone completedMilestone)
        {
            foreach (MilestoneEvent me in milestones)
            {
                if (me.milestone.Equals(completedMilestone) && !me.completed)
                {
                    me.completionEvent?.Invoke();
                    me.completed = true;
                }
            }
        }
        
        
        //================================================================HELPER FUNCTIONS

        public void ModuleShowDialog(string message)
        {
            UIController cont = moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript);
            cont.OpenWindow(UIController.WindowType.Transmission);
            cont.ShowTransmission(message);

        }

        public void ModuleStartTutorial()
        {
            
        }

        public void ModuleFinishTutorial()
        {
            
        }
    }
}
