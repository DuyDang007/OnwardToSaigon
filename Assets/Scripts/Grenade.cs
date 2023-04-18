using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float exploseDelay; // msec
    public float damRange; // meter
    private float sqrDamRange;
    private int damage;
    private bool exploded = false;

    // Transform related
    private float velocity;
    private float vx, vy;
    private float time;
    private float reachTime;

    private ParticleSystem particle;
    private Transform targetTransform;
    private Vector3 originalPosition;
    private PlaySound playSound;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        playSound = GetComponent<PlaySound>();
        sqrDamRange = damRange * damRange;
    }

    // Run this method after creating new grenade instant
    public void initGrenade(Vector3 origPosition, int damageValue, Transform target)
    {
        damage = damageValue;
        this.targetTransform = target;
        originalPosition = origPosition;
        transform.position = originalPosition;
        transform.LookAt(this.targetTransform.position);
        calculateOrbit();
    }

    void calculateOrbit()
    {
        // Calculate start velocity
        float distance = Vector3.Magnitude(targetTransform.position - originalPosition);
        velocity = Mathf.Sqrt(9.81f * distance * 0.5f); // 0.5 * G * d
        vx = velocity; // meter/sec
        vy = velocity; // meter/sec
        time = 0f;
        reachTime = distance / velocity;
    }

    void explode()
    {
        particle.Play();
        playSound.play();
        // Search for all character gameobject
        GameObject[] damageableObject = GameObject.FindGameObjectsWithTag("damageable");
        for(int i = 0; i < damageableObject.Length; i++)
        {
            float distance = Mathf.Abs(damageableObject[i].transform.position.z - transform.position.z);
            if(distance <= damRange)
            {
                damageableObject[i].GetComponent<Character>().takeDamage(damage);
            }
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        if(exploded)
        {
            return;
        }

        if (time < reachTime && Mathf.Abs(transform.position.z - targetTransform.position.z) > 0.4f)
        {
            // Move the grenade to target position
            vy = (velocity - 9.81f * time);
            Vector3 translateVect = new Vector3(0f, vy, vx);
            transform.Translate(translateVect * Time.fixedDeltaTime);
            time += Time.fixedDeltaTime;
        }
        else
        {
            exploded = true;
            explode();
        }
                
    }

    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
