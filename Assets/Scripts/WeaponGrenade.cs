using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : Weapon
{
    public float delayTime;
    public GameObject grenade;
    public Vector3 offsetPosition;
    private float attackTime;

    bool isAttacking;

    public override void attack(Character target, int damage)
    {
        StartCoroutine(spawnGrenade(target, damage));
    }

    IEnumerator spawnGrenade(Character target, int damage)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject grenadeInstance = GameObject.Instantiate(grenade);
        grenadeInstance.GetComponent<Grenade>().initGrenade(transform.position + offsetPosition, damage, target.transform);
    }
}
