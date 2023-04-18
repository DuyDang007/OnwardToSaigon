using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBomb : MonoBehaviour
{
    public GameObject bomb;
    bool isDropping = false;

    IEnumerator dropBomb()
    {
        isDropping = true;
        GameObject bombInst = Instantiate(bomb, transform.position, transform.rotation);
        bombInst.GetComponent<BombDrop>().explode(transform.position);
        yield return new WaitForSeconds(5f);
        isDropping = false;
    }

    private void FixedUpdate()
    {
        if (!isDropping)
        {
            StartCoroutine(dropBomb());
        }
    }
}
