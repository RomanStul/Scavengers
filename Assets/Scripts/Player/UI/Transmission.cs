using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Milestones;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class Transmission : UIWindow
    {
        //================================================================CLASSES
        [Serializable]
        public class TransmissionText
        {
            
            public string key;
            public string[] text;
            public string from;
            public string image;

            public TransmissionText(string key, string[] text)
            {
                this.key = key;
                this.text = text;
            }
        }
        
        [Serializable]
        public class TransmissionWrapper
        {
            public TransmissionText[] texts;
        }
        
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private Text text;
        [SerializeField] private TextMeshProUGUI senderName;
        [SerializeField] private Image senderImage;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
         private TransmissionWrapper transmissions;
         private Sprite[] images;
         private Coroutine writeCoroutine;

         private void Awake()
         {
             transmissions = JsonUtility.FromJson<TransmissionWrapper>(File.ReadAllText("Assets/Json/transmissionText.json"));
             images = Resources.LoadAll<Sprite>("TransmissionImages");
             
         }

         public void WriteMessage(string messageName)
         { 
             int messageIndex = 0;
             int i = 0;
             for (; i < messageName.Length; i++)
             {
                if (System.Char.IsDigit(messageName[i]))
                {
                    messageIndex *= 10;
                    messageIndex += System.Int32.Parse(messageName[i].ToString());
                }
                else
                {
                    break;
                }
                
             }
             
             messageName = messageName.Substring(i+1, messageName.Length - i - 1);

             for (int j = 0; j < transmissions.texts.Length; j++)
             {
                if (transmissions.texts[j].key == messageName)
                { 
                    writeCoroutine = SceneMilestoneManager.currentInstance.StartTrackedCoroutine(Write(transmissions.texts[j].text[messageIndex]));
                    senderName.text = transmissions.texts[j].from;
                    SetImage(transmissions.texts[j].image);
                    break;
                }
             }

         }

         private void SetImage(string imageName)
         {
             for (int i = 0; i < images.Length; i++)
             {
                 if (images[i].name.Equals(imageName))
                 {
                     senderImage.sprite = images[i];
                     return;
                 }
             }

             senderImage.sprite = null;
         }

         private IEnumerator Write(string message)
         {
             text.text = "";
             char space = " "[0];
             for (int i = 0; i < message.Length && transform.gameObject.activeSelf; i++)
             {
                 text.text += message[i];
                 yield return new WaitForSeconds(Char.IsWhiteSpace(message[i]) ? 0.05f : 0.025f);
             }

             yield return new WaitUntil(() => !transform.gameObject.activeSelf);

         }
    }
}
