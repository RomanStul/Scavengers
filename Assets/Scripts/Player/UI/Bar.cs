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
        }
    }
}
