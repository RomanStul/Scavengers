using System;
using System.Collections.Generic;
using Player.Module;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Milestones
{
    public class GlobalMilestoneManager : MonoBehaviour
    {
        //================================================================CLASSES4
        
        [Serializable]
        public enum MilestoneAction
        {
            Entered,
            PickedUp,
            Destroyed,
            Upgraded,
            Day,
            TimerRanOut
        }
        
        [Serializable]
        public class Milestone
        {
            public Milestone(MilestoneAction a, int id)
            {
                this.action = a;
                this.originID = id;
            }

            public override bool Equals(Object other)
            {
                return ((Milestone)other).action == action && originID == ((Milestone)other).originID;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine((int)action, originID);
            }

            public MilestoneAction action;
            public int originID;
        }
        
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        public Module ModuleRef
        {
            get { return moduleRef; }
            set { moduleRef = value; }
        }

        public Dictionary<string, List<Milestone>> CompletedMilestones
        {
            get { return completedMilestones; }
            set { completedMilestones = value; }
        }

        public List<Milestone> UnclaimedMilestones
        {
            get => unclaimedMilestones;
            set => unclaimedMilestones = value;
        }
        //================================================================FUNCTIONALITY
        public static GlobalMilestoneManager instance;
        private Dictionary<string, List<Milestone>> completedMilestones = new Dictionary<string, List<Milestone>>();
        private List<Milestone> unclaimedMilestones = new List<Milestone>();
        private Module moduleRef;
        
        private void Awake()
        {
            completedMilestones.Add(SceneManager.GetActiveScene().name, new List<Milestone>());
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsMilestoneCompleted(Milestone milestone)
        {
            foreach (Milestone m in completedMilestones[SceneManager.GetActiveScene().name])
            {
                if (m.Equals(milestone))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddCompletedMilestone(Milestone milestone)
        {
            completedMilestones[SceneManager.GetActiveScene().name].Add(milestone);
        }

        public void AddUnclaimedMilestone(Milestone milestone)
        {
            if (unclaimedMilestones.Contains(milestone))
            {
                return;
            }
            unclaimedMilestones.Add(milestone);
        }
        
        

        public bool AddSceneDictionary()
        {
            bool ret = true;
            
            if (!completedMilestones.ContainsKey(SceneManager.GetActiveScene().name))
            {
                completedMilestones.Add(SceneManager.GetActiveScene().name, new List<Milestone>());
                ret = false;
            }

            return ret;
        }

        public void ClaimUnclaimedMilestones()
        {
            for (int i = 0; i < unclaimedMilestones.Count; i ++)
            {
                if (SceneMilestoneManager.currentInstance.CompletedMilestone(unclaimedMilestones[i], true))
                {
                    unclaimedMilestones.RemoveAt(i);
                }
            }
        }
    }
}
