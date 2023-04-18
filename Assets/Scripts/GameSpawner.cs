using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    public GameSpawner opponentSpawner;
    public int money;
    public GameObject[] characterPrefab;
    public float[] cooldownTime;
    public int[] characterPrice;
    //public Vector3 spawnPosition;
    public float randomSpawnOffset;
    public Character baseObject;

    [Header("Do not set this field")]
    public float[] respawnTime; // Time to update every frame, reaching zero mean respawnable

    private int baseMaxHealth;

    /* Delegate methods */
    public void enemyDead(int enemyReward)
    {
        money += enemyReward;
    }


    /* Public methods */
    public void spawnCharacter(GameStatus.CharacterClassE characterClass)
    {
        float spawnOffset = Random.Range(-randomSpawnOffset, randomSpawnOffset);
        GameObject characterObject = Instantiate(characterPrefab[(int)characterClass]);
        //characterObject.transform.position = spawnPosition + new Vector3(spawnOffset, 0f, 0f);
        characterObject.transform.position = transform.position + new Vector3(spawnOffset, 0f, 0f);
        characterObject.GetComponent<Character>().enemySpawner = opponentSpawner;
        respawnTime[(int)characterClass] = cooldownTime[(int)characterClass];
    }

    public GameStatus.BuyStatusE Buy(GameStatus.CharacterClassE characterClass)
    {
        // Get character price
        int price = characterPrice[(int)characterClass];

        if(price > money || respawnTime[(int)characterClass] > 0)
        {
            return GameStatus.BuyStatusE.Fail;
        }

        // Otherwise
        money = money - price;
        spawnCharacter(characterClass);
        return GameStatus.BuyStatusE.Success;
    }

    public bool canBuy(GameStatus.CharacterClassE charClass)
    {
        if (characterPrice[(int)charClass] > money || respawnTime[(int)charClass] > 0)
        {
            return false;
        }
        return true;
    }

    public int getBaseHealth()
    {
        return baseObject.health;
    }
    public int getMaxHealth()
    {
        return baseMaxHealth;
    }

    private void Start()
    {
        respawnTime = new float[characterPrefab.Length];
        for(int i = 0; i < respawnTime.Length; i++)
        {
            respawnTime[i] = 0;
        }

        baseMaxHealth = baseObject.health;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < respawnTime.Length; i++)
        {
            if(respawnTime[i] > 0)
            {
                respawnTime[i] -= Time.fixedDeltaTime;
            }
        }
    }
}
