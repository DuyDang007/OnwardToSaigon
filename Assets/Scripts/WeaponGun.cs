using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGun : Weapon
{
    public Gun gunMuzzle;

    public override void attack(Character target, int damage)
    {
        gunMuzzle.fireAt(target.transform);
        target.takeDamage(damage);
    }
}
