using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameSpawner redSpawner;
    public GameSpawner yellowSpawner;
    bool isSpawning = false;

    IEnumerator spawn()
    {
        isSpawning = true;
        redSpawner.Buy((GameStatus.CharacterClassE)Random.Range(0, 4));
        yellowSpawner.Buy((GameStatus.CharacterClassE)Random.Range(0, 4));
        yield return new WaitForSeconds(3f);
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpawning)
        {
            StartCoroutine(spawn());
        }
    }
}
