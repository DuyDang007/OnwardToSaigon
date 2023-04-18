using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillToggleButton : BuyButton
{
    private Toggle toggleBtn;
    // Start is called before the first frame update
    void Awake()
    {
        toggleBtn = GetComponent<Toggle>();
    }

    public override void activateButton(bool yes)
    {
        if (yes)
        {
            toggleBtn.interactable = true;
        }
        else
        {
            toggleBtn.interactable = false;
        }
    }
}
