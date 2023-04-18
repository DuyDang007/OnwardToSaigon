using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : MonoBehaviour
{
    public float damageRange;
    public int damage;
    public ParticleSystem explosionParticle;
    public PlaySound playSound;

    public void explode(Vector3 position)
    {
        
        // Search for all character gameobject
        GameObject[] damageableObject = GameObject.FindGameObjectsWithTag("damageable");
        for (int i = 0; i < damageableObject.Length; i++)
        {
            float sqrDistance = Mathf.Abs(damageableObject[i].transform.position.z - transform.position.z);
            if (sqrDistance <= damageRange)
            {
                damageableObject[i].GetComponent<Character>().takeDamage(damage);
            }
        }
        StartCoroutine(explosionCoroutine(position));
    }
    IEnumerator explosionCoroutine(Vector3 position)
    {
        Vector3[] offset = {new Vector3(Random.Range(-10f, 10f), 0f, -damageRange),
                            new Vector3(Random.Range(-10f, 10f), 0f, -damageRange * 0.3f),
                            new Vector3(Random.Range(-10f, 10f), 0f, damageRange * 0.3f),
                            new Vector3(Random.Range(-10f, 10f), 0f, damageRange)};
        for(int i = 0; i < offset.Length; i++)
        {
            explosionParticle.gameObject.transform.localPosition = offset[i];
            explosionParticle.Play(true);
            playSound.play();
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
