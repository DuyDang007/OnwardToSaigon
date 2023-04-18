using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCharacter : Character
{
    public GameObject flame;

    public override void takeDamage(int dam)
    {
        base.takeDamage(dam);
        if(isDead)
        {
            GameObject.Instantiate(flame, transform.position, Quaternion.identity);
        }
    }

    // Same as base method, but the moving direction is different
    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        // Search for enemy when no target accquired
        if (!isSearching)
        {
            StartCoroutine(searchForEnemy());
        }

        if(enemyTarget != null && !enemyTarget.isDead)
        {
            // When enemy in range and not attacking: Attack
            if (Mathf.Abs(enemyTarget.transform.position.z - transform.position.z) <= attackRange)
            {
                if (!isAttacking)
                {
                    objAnimator.SetFloat("walkspeed", 0f);
                    StartCoroutine(attackCoroutine(enemyTarget));
                }
            }
            // Otherwise, move forward
            else
            {
                objAnimator.SetFloat("walkspeed", 1f);
                transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
            }
        }
        // No more enemy found: Stop moving
        else
        {
            objAnimator.SetFloat("walkspeed", 0f);
        }

    }

}
