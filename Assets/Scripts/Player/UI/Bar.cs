using UnityEngine;

namespace Player.UI
{
    public class Bar : MonoBehaviour
    {
        //================================================================CLASSES
    
        //================================================================EDITOR VARIABLES
        [SerializeField] protected float value, maxValue;
        [SerializeField] protected RectTransform thisRect, fill;
        [SerializeField] protected float minFillY, maxFillY;
        //TODO add outline for full bar and cross for empty
        //================================================================GETTER SETTER
        public virtual void SetValue(float barValue)
        {
            value = barValue;
            UpdateFill();
        }

        public virtual void SetMaxValue(float barValue)
        {
            maxValue = barValue;
        }
        //================================================================FUNCTIONALITY

        protected virtual void UpdateFill()
        {
            fill.anchoredPosition = new Vector2(fill.anchoredPosition.x, (value/maxValue * (maxFillY - minFillY)) + minFillY);
            return;
            
            float yValue = (thisRect.position.y + thisRect.sizeDelta.y * (value / maxValue));
            //Debug.Log(thisRect.position.y + " + " + thisRect.sizeDelta.y + " * " + (value / maxValue) + " = " +yValue);
            fill.position = new Vector2(fill.position.x, yValue);
        }
    }
}
