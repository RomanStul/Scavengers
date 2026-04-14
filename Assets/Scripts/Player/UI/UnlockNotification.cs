using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.UI
{
    public class UnlockNotification : MonoBehaviour
    {
        //================================================================CLASSES
        public class NotificationObject
        {
            public bool isTool = false;
            public string unlockName;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private TextMeshProUGUI text;
        //================================================================GETTER SETTER
        public void AddToNotification(string unlockName, bool isTool)
        {
            notificationQueue.Enqueue(new NotificationObject(){isTool = isTool, unlockName = unlockName});
            if (!isPrinting) StartCoroutine(PrintNotifications());
        }
        //================================================================FUNCTIONALITY
        private Queue<NotificationObject> notificationQueue = new Queue<NotificationObject>();
        private bool isPrinting = false;
        
        public IEnumerator PrintNotifications()
        {
            isPrinting = true;
            while (notificationQueue.Count > 0)
            {
                StringBuilder s = new StringBuilder();
                NotificationObject obj = notificationQueue.Dequeue();
                if (obj.isTool)
                {
                    text.text = "New tool unlocked\n" + obj.unlockName;
                }
                else
                {
                    text.text = "New Upgrade unlocked\n" + obj.unlockName;
                }
                yield return new WaitForSeconds(2f);
                text.text = "";
                yield return new WaitForSeconds(0.6f);
            }
            
            text.text = "";
        }
    }
}
