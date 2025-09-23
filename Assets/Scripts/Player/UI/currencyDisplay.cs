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
        [SerializeField] private Sprite dot;
        [SerializeField] private Sprite minus;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public void DisplayBalance(int balance)
        {
            //TODO display by thousands and millions
            int i = 0;
            int used = 0;
            if (balance < 0)
            {
                displaySegments[^1].sprite = minus;
                balance = -balance;
                used = 1;
                displaySegments[^1].enabled = true;
            }
            while (i < displaySegments.Length - used)
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
