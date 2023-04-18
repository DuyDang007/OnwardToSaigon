using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrenade : MonoBehaviour
{
    public GameObject grenade;
    public Transform endPoint;

    private float velocity;
    private float vx, vy;
    private float time;

    void Calc()
    {
        // Calculate start velocity
        float distance = Vector3.Magnitude(endPoint.position - transform.position);
        velocity = Mathf.Sqrt(9.81f * distance * 0.5f); // 0.5 * G * d
        vx = velocity; // meter/sec
        vy = velocity; // meter/sec
        time = 0f;

        // Set grenade start posistion
        grenade.transform.position = gameObject.transform.position;
        grenade.transform.LookAt(endPoint.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        Calc();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vy = (velocity - 9.81f * time);
        Vector3 translateVect = new Vector3(0f, vy, vx);
        grenade.transform.Translate(translateVect * Time.fixedDeltaTime);
        time += Time.fixedDeltaTime;

        if(Mathf.Abs(grenade.transform.position.z - endPoint.position.z) < 1f)
        {
            Calc();
        }
    }
}
