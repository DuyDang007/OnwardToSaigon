using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : Character
{
    public float xMinRand, xMaxRand, yMinRand, yMaxRand, zMinRand, zMaxRand;
    public GameObject finalExplosion;

    private bool isMoveDone = true;
    public override void takeDamage(int dam)
    {
        health -= dam;
        if(health <= 0 && !isDead)
        {
            isDead = true;
            Instantiate(finalExplosion, transform.position, Quaternion.identity);
        }
    }

    IEnumerator randomMoveCoroutine()
    {
        isMoveDone = false;
        Vector3 randPosition = new Vector3(
            Random.Range(xMinRand, xMaxRand),
            Random.Range(yMinRand, yMaxRand),
            Random.Range(zMinRand, zMaxRand));
        transform.localPosition = randPosition;
        yield return new WaitForSeconds(1f);
        isMoveDone = true;
    }

    private void Awake()
    {
        return;
    }
    private void FixedUpdate()
    {
        // Move the character randomly
        if(isMoveDone)
        {
            StartCoroutine(randomMoveCoroutine());
        }
    }

}
