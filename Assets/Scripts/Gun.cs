using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // This object is for effect only, not the actual damage

    [SerializeField]
    private float bulletSpeed;
    public GameObject bulletRay;
    protected ParticleSystem gunParticle;
    ParticleSystem bulletParticle;
    protected PlaySound playSound;

    private void Awake()
    {
        gunParticle = GetComponent<ParticleSystem>();
        bulletParticle = bulletRay.GetComponent<ParticleSystem>();
        playSound = GetComponent<PlaySound>();
    }

    public virtual void fireAt(Transform target)
    {
        bulletRay.transform.LookAt(target.position + Vector3.up * 1.5f);
        // Calculate life time for a single bullet base on target distance
        float lifeTime = Vector3.Magnitude(target.position - transform.position) / bulletSpeed;
        var bulletPar = bulletParticle.main;
        var bulletParVelocity = bulletParticle.velocityOverLifetime;
        bulletParVelocity.z = bulletSpeed;
        bulletPar.startLifetime = lifeTime;
        gunParticle.Play();
        playSound.play();
    }
}
