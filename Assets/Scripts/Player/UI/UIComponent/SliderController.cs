using UnityEngine;
using UnityEngine.UI;

namespace Player.UI.UIComponent
{
    public class SliderController : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private Slider slider;
        
        //================================================================GETTER SETTER

        public void SetMaxValue(int maxValue)
        {
            slider.maxValue = maxValue;
        }
        
        //================================================================FUNCTIONALITY

        public void MoveSlider(int offset)
        {
            slider.value += offset;
        }
    }
}
