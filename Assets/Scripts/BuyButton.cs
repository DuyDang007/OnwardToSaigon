using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public Image inactiveMask;
    public Text priceText;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void updateFillAmount(float amount)
    {
        if(amount > 1f || amount < 0f)
        {
            Debug.LogError("Invalid fill amount " + amount);
        }
        else
        {
            inactiveMask.fillAmount = amount;
        }
    }

    public float fillAmount()
    {
        return inactiveMask.fillAmount;
    }

    public virtual void activateButton(bool yes)
    {
        if(yes)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void setPrice(int price)
    {
        priceText.text = price.ToString();
    }
}
