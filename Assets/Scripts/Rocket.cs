using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform rocket;
    ParticleSystem rocketParticle;
    ParticleSystem particle;
    private PlaySound playSound;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        rocketParticle = rocket.gameObject.GetComponent<ParticleSystem>();
        playSound = GetComponent<PlaySound>();
    }

    public void fireAt(Transform target)
    {
        rocket.LookAt(target.position + new Vector3(0f, 1.5f, 0f));

        // Calculate rocket explosion time
        float distance = Vector3.Magnitude(transform.position - target.position);
        var rocketMainSetting = rocketParticle.main;
        var rocketVelocitySetting = rocketParticle.velocityOverLifetime;
        float rocketSpeed = rocketVelocitySetting.z.constant;
        rocketMainSetting.startLifetime = distance / rocketSpeed;

        particle.Play();
        playSound.play();
    }
}
