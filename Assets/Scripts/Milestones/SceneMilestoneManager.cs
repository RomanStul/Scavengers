using System;
using System.Collections;
using System.Collections.Generic;
using Player.Module;
using Player.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;
using Environment = Entities.Environment.Environment;
using Milestones;

namespace Milestones
{
    public class SceneMilestoneManager : MonoBehaviour
    {
        //================================================================CLASSES

        [Serializable]
        public class EventListElement
        {
            public UnityEvent completeEvent;
            public bool wait;
        }

        [Serializable]
        public class MilestoneEvent
        {
            public GlobalMilestoneManager.Milestone milestone;
            public List<EventListElement> completionEventList;
            public bool completed = false;
        }
        
        //================================================================EDITOR VARIABLES
        [SerializeField] private List<MilestoneEvent> milestones;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private Module moduleRef;
        private Coroutine executeCoroutine;
        public static SceneMilestoneManager currentInstance;


        private void Awake()
        {
            List<MilestoneEvent> unCompletedMilestones = new List<MilestoneEvent>();
            
            currentInstance = this;
            foreach (MilestoneEvent me in milestones)
            {
                if (!GlobalMilestoneManager.instance.IsMilestoneCompleted(me.milestone))
                {
                    unCompletedMilestones.Add(me);
                }
            }
            
            milestones = unCompletedMilestones;
            moduleRef = GlobalMilestoneManager.instance.ModuleRef;
        }


        public void CompletedMilestone(GlobalMilestoneManager.Milestone completedMilestone)
        {
            foreach (MilestoneEvent me in milestones)
            {
                if (me.milestone.Equals(completedMilestone) && !me.completed)
                {
                    me.completed = true;
                    GlobalMilestoneManager.instance.AddCompletedMilestone(me.milestone);
                    StartCoroutine(InvokeMilestoneEvents(me));
                }
            }
        }

        private IEnumerator InvokeMilestoneEvents(MilestoneEvent me)
        {
            for (int i = 0; i < me.completionEventList.Count; i++)
            {
                me.completionEventList[i].completeEvent?.Invoke();
                
                if (me.completionEventList[i].wait)
                {
                    yield return WaitForTrackedCoroutine();
                }
            }

            yield return null;
        }
        
        public Coroutine StartTrackedCoroutine(IEnumerator routine)
        {
            executeCoroutine = StartCoroutine(routine);
            return executeCoroutine;
        }

        private IEnumerator WaitForTrackedCoroutine()
        {
            if (executeCoroutine != null)
                yield return executeCoroutine;
        }
        
        
        //================================================================HELPER FUNCTIONS

        public void ModuleShowDialog(string message)
        {
            UIController cont = moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript);
            cont.OpenWindow(UIController.WindowType.Transmission);
            cont.ShowTransmission(message);

        }

        public void ModuleHelpWindowMode(int mode)
        {
            UIController cont = moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript);
            cont.SetHelpWindowMode((HelpDisplay.DisplayModes)mode);
        }

        public void OpenWindow(int windowType)
        {
            moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow((UIController.WindowType)windowType);
        }

        public void CloseSpecificWindow(int windowType)
        {
            moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).CloseSpecificWindow((UIController.WindowType)windowType);
        }

        public void ModuleStartTutorial()
        {
            Module.EvacuateSettings settings = moduleRef.GetEvacuateSettings();
            settings.evacuateToSameScene = true;
            settings.costMultiplier = 0;
            settings.evacuatePosition = moduleRef.transform.position;
            Environment.instance.damageMultiplier = 1;
            HealthBar healthBar = moduleRef.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript);
            healthBar.SetHealth(0);
        }   

        public void ModuleFinishTutorial()
        {
            Module.EvacuateSettings settings = moduleRef.GetEvacuateSettings();
            settings.evacuateToSameScene = false;
            settings.costMultiplier = 1;
            settings.evacuatePosition = Vector3.zero;
            Environment.instance.damageMultiplier = 0;
            HealthBar healthBar = moduleRef.GetScript<HealthBar>(Module.ScriptNames.HealthBarScript);
            healthBar.HealHealth();
        }

        public void ModuleStop(Transform stopAt)
        {
            StartTrackedCoroutine(GradualModuleStop(stopAt.position));
        }

        private IEnumerator GradualModuleStop(Vector3 position)
        {
            while (true)
            {
                Vector2 direction = Convertor.Vec3ToVec2(position - moduleRef.transform.position);
                moduleRef.moveRb.velocity = direction.normalized * 0.5f + moduleRef.moveRb.velocity.normalized * 0.5f;
                moduleRef.moveRb.angularVelocity *= 0.8f;
                if (direction.magnitude < 1f)
                {
                    if (direction.magnitude < 0.2)
                    {
                        break;
                    }
                    moduleRef.moveRb.velocity *= Mathf.Max(direction.magnitude, 0.5f);
                }
                yield return new WaitForSeconds(0.05f);
            }
            moduleRef.moveRb.velocity = Vector3.zero;
            moduleRef.moveRb.angularVelocity = 0;
        }
    }
}
