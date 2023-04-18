using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Gun
{
    private void Awake()
    {
        gunParticle = GetComponent<ParticleSystem>();
        playSound = GetComponent<PlaySound>();
    }

    public override void fireAt(Transform target)
    {
        bulletRay.transform.position = target.position;
        gunParticle.Play();
        playSound.play();
    }
}
