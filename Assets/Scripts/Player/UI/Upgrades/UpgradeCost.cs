using UnityEngine;
using UnityEngine.UI;

public class UpgradeCost : MonoBehaviour
{
    //================================================================CLASSES
    //================================================================EDITOR VARIABLES

    [SerializeField] private Text costAmount;

    [SerializeField] private Image icon;
    //================================================================GETTER SETTER

    public void SetCostAmount(int cost)
    {
        costAmount.text = cost.ToString() + "x";
    }

    public void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
    }
    
    //================================================================FUNCTIONALITY

    public void ShowResourceAvailable(bool available)
    {
        costAmount.color = available ? Color.white : Color.red;
    }
}
