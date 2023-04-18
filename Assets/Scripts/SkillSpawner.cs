using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject bombDrop;
    public bool isAiming = false;
    public float cooldownTimeConst;
    public float respawnTime;

    public void activeAim()
    {
        arrow.SetActive(true);
        isAiming = true;
    }
    public void deactiveAim()
    {
        arrow.SetActive(false);
        isAiming = false;
    }

    public void spawn(Vector3 position)
    {
        respawnTime = cooldownTimeConst;
        StartCoroutine(dropBomb(position));
    }

    IEnumerator dropBomb(Vector3 position)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject bomb = Instantiate(bombDrop, arrow.transform.position, Quaternion.identity);
        bomb.GetComponent<BombDrop>().explode(bomb.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        arrow.SetActive(false);
        respawnTime = 0f;
    }

    private void Update()
    {
        if(isAiming)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000f))
            {
                arrow.transform.position = new Vector3(0f, 0f, hit.point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Skill cast");
                    spawn(arrow.transform.position);
                }
            }
        }
        if (respawnTime > 0f)
        {
            respawnTime -= Time.deltaTime;
        }
        else respawnTime = 0f;
    }
}
