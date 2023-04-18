using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRocket : Weapon
{
    public Rocket rocket;
    public float damageRadius;

    public override void attack(Character target, int damage)
    {
        rocket.fireAt(target.transform);
        StartCoroutine(causeDamageCouroutine(target, damage));
    }

    IEnumerator causeDamageCouroutine(Character target, int damage)
    {
        yield return new WaitForSeconds(0.2f);
        GameObject[] damageableObject = GameObject.FindGameObjectsWithTag("damageable");
        for (int i = 0; i < damageableObject.Length; i++)
        {
            float sqrDistance = Vector3.SqrMagnitude(damageableObject[i].transform.position - target.transform.position);
            if (sqrDistance <= damageRadius * damageRadius)
            {
                damageableObject[i].GetComponent<Character>().takeDamage(damage);
            }
        }
    }
}
