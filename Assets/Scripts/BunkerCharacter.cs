using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerCharacter : Character
{
    public GameObject explosion;

    public override void takeDamage(int dam)
    {
        health -= dam;
        if (health <= 0)
        {
            if (!isDead)
            {
                StartCoroutine(deadCoroutine());
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        // Search for nearest enemy
        if (!isSearching)
        {
            StartCoroutine(searchTarget());
        }

        // Attack when enemy in range
        if (enemyTarget != null && !enemyTarget.isDead)
        {
            if (!isAttacking && Mathf.Abs(enemyTarget.transform.position.z - transform.position.z) <= attackRange)
            {
                StartCoroutine(attackCoroutine(enemyTarget));
            }
        }
    }

    IEnumerator deadCoroutine()
    {
        isDead = true;
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5f);
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }

    new public IEnumerator attackCoroutine(Character target)
    {
        isAttacking = true;
        weapon.attack(target, damage);
        yield return new WaitForSeconds(attackCooldownTime);
        isAttacking = false;
    }

    IEnumerator searchTarget()
    {
        isSearching = true;
        GameObject[] targetArray = GameObject.FindGameObjectsWithTag("damageable");
        float minDistance = 1000f;
        for (int i = 0; i < targetArray.Length; i++)
        {
            // Target is other side and has minimum distance to character
            if (targetArray[i].GetComponent<Character>().side != side && !targetArray[i].GetComponent<Character>().isDead &&
                Mathf.Abs(targetArray[i].transform.position.z - transform.position.z) < minDistance)
            {
                enemyTarget = targetArray[i].GetComponent<Character>();
                minDistance = Mathf.Abs(targetArray[i].transform.position.z - transform.position.z);
            }
        }
        yield return new WaitForSeconds(1f);
        isSearching = false;
    }
}
