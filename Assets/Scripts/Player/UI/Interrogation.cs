using System;
using System.Collections;
using System.IO;
using Player.Module;
using story;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

namespace Player.UI
{
    public class Interrogation : MonoBehaviour
    {
        //================================================================CLASSES
        [Serializable]
        public class InterrogationTexts
        {
            public string[] texts;
        }
        
        
        public enum InterrogationName
        {
            Day0,
            FailedToPay
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private TextMeshProUGUI paragraphPrefab;

        [SerializeField] private VerticalLayoutGroup textHolder;

        [SerializeField] private GameObject continueButton, mainMenuButton;
        //================================================================GETTER SETTER

        public void SetScrollFalse()
        {
            scroll = false;
        }
        //================================================================FUNCTIONALITY
        private InterrogationTexts interrogationTexts;
        private float targetOffset = 0;
        private bool scroll = true;

        private void Start()
        {
            interrogationTexts = JsonUtility.FromJson<InterrogationTexts>(File.ReadAllText("Assets/Json/InterrogationTexts.json"));
        }

        public void Write(InterrogationName textId, Module.Module moduleRef)
        {
            StartCoroutine(Printing(interrogationTexts.texts[(int)textId], moduleRef));
        }

        private IEnumerator Printing(string text, Module.Module moduleRef)
        {
            TextMeshProUGUI currentParagraph = Instantiate(paragraphPrefab, textHolder.transform);
            currentParagraph.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                currentParagraph.text += text[i];
                yield return new WaitForEndOfFrame();
                ((RectTransform)currentParagraph.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentParagraph.textInfo.lineCount * currentParagraph.fontSize);
                yield return new WaitForSeconds(Char.IsWhiteSpace(text[i]) ? 0.02f : 0.06f);
                
                if (text[i] == '\n')
                {
                    yield return new WaitForSeconds(0.7f);
                    if (targetOffset == 0)
                    {
                        scroll = true;
                        StartCoroutine(Scroll());
                    }
                    targetOffset += ((RectTransform)currentParagraph.transform).rect.height + textHolder.spacing;
                    
                    currentParagraph = Instantiate(paragraphPrefab, textHolder.transform);
                    currentParagraph.text = "";
                }
                
            }
            
            ((ModuleAnimations)moduleRef.GetScript<ModuleAnimations>(Module.Module.ScriptNames.AnimationFunctionsScript)).PlayNextInterrogationAnimation();
        }

        public void ShowButtons()
        {
            continueButton.SetActive(true);
            mainMenuButton.SetActive(true);
        }

        private IEnumerator Scroll()
        {
            bool previsouslyNotScrolling = true;
            while (scroll)
            {
                if (targetOffset > ((RectTransform)textHolder.transform).anchoredPosition.y)
                {
                    if (previsouslyNotScrolling)
                    {
                        previsouslyNotScrolling = false;
                        yield return new WaitForSeconds(0.5f);
                    }
                    ((RectTransform)textHolder.transform).anchoredPosition += new Vector2(0, Time.deltaTime * 300);
                }
                else
                {
                    previsouslyNotScrolling = true;
                }

                yield return null;
            }

            Debug.Log("scroll ended");
        }
    }
}
