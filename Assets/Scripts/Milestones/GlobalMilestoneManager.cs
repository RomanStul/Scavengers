using System;
using System.Collections.Generic;
using Player.Module;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Day
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
        //================================================================FUNCTIONALITY
        public static GlobalMilestoneManager instance;
        private Dictionary<string, List<Milestone>> completedMilestones = new Dictionary<string, List<Milestone>>();
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

        public bool AddSceneDictionary()
        {
            if (!completedMilestones.ContainsKey(SceneManager.GetActiveScene().name))
            {
                completedMilestones.Add(SceneManager.GetActiveScene().name, new List<Milestone>());
                return false;
            }

            return true;
        }
    }
}
