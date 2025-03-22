using UnityEngine;

namespace Player.UI
{
    public class Bar : MonoBehaviour
    {
        //================================================================CLASSES
    
        //================================================================EDITOR VARIABLES
        [SerializeField] private float value, maxValue;
        [SerializeField] private RectTransform thisRect, fill;
        //TODO add outline for full bar and cross for empty
        //================================================================GETTER SETTER
        public void SetValue(int barValue)
        {
            value = barValue;
            UpdateFill();
        }

        public void SetMaxValue(int barValue)
        {
            maxValue = barValue;
        }
        //================================================================FUNCTIONALITY

        private void UpdateFill()
        {
            //TODO fix that bars seem empty first
            float yValue = (thisRect.anchoredPosition.y + thisRect.sizeDelta.y * (value / maxValue));
            Debug.Log(thisRect.anchoredPosition.y + " + " + thisRect.sizeDelta.y + " * " + (value / maxValue) + " = " +yValue);
            fill.anchoredPosition = new Vector2(fill.anchoredPosition.x, yValue);
        }
    }
}
