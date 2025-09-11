using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class CurrencyDisplay : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        
        [SerializeField] private Sprite[] numbers;
        [SerializeField] private Image[] displaySegments;
        [SerializeField] private Image dot;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public void DisplayBalance(int balance)
        {
            //TODO display by thousands and millions and negative
            int i = 0;
            if (balance > 0) balance = 0;
            while (i < displaySegments.Length)
            {
                if (balance > 0 || i == 0)
                {
                    displaySegments[i].sprite = numbers[balance%10];
                    balance /= 10;
                    displaySegments[i].enabled = true;
                }
                else
                {
                    displaySegments[i].enabled = false;
                }
                i++;
            }
        }
    }
}
