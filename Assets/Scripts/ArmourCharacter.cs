using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCharacter : Character
{
    public GameObject flame;
    public override void takeDamage(int dam)
    {
        base.takeDamage(dam);
        if (isDead)
        {
            GameObject.Instantiate(flame, transform.position, Quaternion.identity);
        }
    }
}
