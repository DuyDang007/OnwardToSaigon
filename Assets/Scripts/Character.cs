using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameStatus.SideE side;
    public int health;
    public int damage;
    public float moveSpeed;
    public float attackSpeed; // Time per sec
    public float attackRange;
    //public int price;
    public int reward;
    public bool isDead = false;
    public GameSpawner enemySpawner;
    public bool isStaticObject = false;

    protected float attackCooldownTime; // ms
    protected bool isAttacking = false;
    protected bool isSearching = false;
    protected Character enemyTarget;

    [SerializeField]
    protected Weapon weapon;

    protected Animator objAnimator;
    

    public virtual void takeDamage(int dam)
    {
        health -= dam;
        if(health <= 0)
        {
            if(!isDead)
            {
                StartCoroutine(deadCoroutine());
            }
        }
    }

    private IEnumerator deadCoroutine()
    {
        int deadRand = Random.Range(1, 4);
        objAnimator.SetInteger("dead_random", deadRand);
        objAnimator.SetTrigger("dead");
        isDead = true;

        // Give reward to opponent
        enemySpawner.enemyDead(reward);

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public IEnumerator attackCoroutine(Character target)
    {
        isAttacking = true;
        weapon.attack(target, damage);
        objAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldownTime);
        isAttacking = false;
    }

    private void Awake()
    {
        objAnimator = GetComponent<Animator>();
        attackCooldownTime = 1f / attackSpeed; // sec
    }

    private void FixedUpdate()
    {
        if(isDead)
        {
            return;
        }

        // Search for nearest enemy
        if (!isSearching)
        {
            StartCoroutine(searchForEnemy());
        }

        if (enemyTarget != null && !enemyTarget.isDead)
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
                transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
            }
        }
        // No more enemy found: Stop moving
        else
        {
            objAnimator.SetFloat("walkspeed", 0f);
        }
    }

    protected IEnumerator searchForEnemy()
    {
        isSearching = true;
        GameObject[] targetArray = GameObject.FindGameObjectsWithTag("damageable");
        float minDistance = 1000f;
        for(int i = 0; i < targetArray.Length; i++)
        {
            // Target is other side and has minimum distance to character
            if(targetArray[i].GetComponent<Character>().side != side && !targetArray[i].GetComponent<Character>().isDead &&
                Mathf.Abs(targetArray[i].transform.position.z - transform.position.z) < minDistance)
            {
                enemyTarget = targetArray[i].GetComponent<Character>();
                minDistance = Mathf.Abs(targetArray[i].transform.position.z - transform.position.z);
            }
        }
        yield return new WaitForSeconds(0.5f);
        isSearching = false;
    }
}
