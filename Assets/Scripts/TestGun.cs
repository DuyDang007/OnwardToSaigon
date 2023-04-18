using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    public Rocket gun;
    public GameObject target;

    bool isFiring = false;

    // Update is called once per frame
    void Update()
    {
        if(!isFiring)
            StartCoroutine(fire(target.transform));
    }

    IEnumerator fire(Transform target)
    {
        isFiring = true;
        gun.fireAt(target);
        yield return new WaitForSeconds(2f);
        isFiring = false;
    }
}
