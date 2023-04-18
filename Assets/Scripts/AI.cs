using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameSpawner charSpawner;
    public SkillSpawner skillSpawner;
    public BaseCharacter baseCharacter;
    public bool enableGunner, enableGrenadier, enableSniper, enableRocket, enableArmour, enableTank, enableSkill;
    public float skillRangeZMin, skillRangeZMax;

    private bool isBuying = false;
    private bool isCastingSkill = false;


    // Update is called once per frame
    void Update()
    {
        if (baseCharacter.health > 0)
        {
            if (!isBuying)
            {
                StartCoroutine(buyRandom());
            }
            if (!isCastingSkill && enableSkill)
            {
                StartCoroutine(castSkill());
            }
        }
        
    }

    IEnumerator buyRandom()
    {
        isBuying = true;
        GameStatus.BuyStatusE buyStatus = GameStatus.BuyStatusE.Fail;

        // Random to buy character
        int randomInt = Random.Range(0, 100);
        if (randomInt < 25 && enableGunner)
        {
            buyStatus = charSpawner.Buy(GameStatus.CharacterClassE.Gunner);
        }
        else if (randomInt < 50 && enableGrenadier)
        {
            buyStatus = charSpawner.Buy(GameStatus.CharacterClassE.Grenadier);
        }
        else if (randomInt < 65 && enableSniper)
        {
            buyStatus = charSpawner.Buy(GameStatus.CharacterClassE.Sniper);
        }
        else if (randomInt < 75 && enableRocket)
        {
            buyStatus = charSpawner.Buy(GameStatus.CharacterClassE.Rocket);
        }
        else if (randomInt < 85 && enableArmour)
        {
            buyStatus = charSpawner.Buy(GameStatus.CharacterClassE.Armour);
        }
        else if (randomInt < 100 && enableTank)
        {
            buyStatus = charSpawner.Buy(GameStatus.CharacterClassE.Tank);
        }
        if(buyStatus == GameStatus.BuyStatusE.Success)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(0.7f);
        }

        isBuying = false;
    }

    IEnumerator castSkill()
    {
        isCastingSkill = true;
        // Use skill when enemy force is too high compared to own's force
        List<Character> redList     = new List<Character>();
        List<Character> yellowList  = new List<Character>();

        GameObject[] charObjectArray = GameObject.FindGameObjectsWithTag("damageable");

        // Sort all character to 2 sides
        for (int i = 0; i < charObjectArray.Length; i++)
        {
            if (charObjectArray[i].GetComponent<Character>().isStaticObject == false)
            { 
                if(charObjectArray[i].GetComponent<Character>().side == GameStatus.SideE.Red)
                {
                    redList.Add(charObjectArray[i].GetComponent<Character>());
                }
                else //(charObjectArray[i].GetComponent<Character>().side == GameStatus.SideE.Yellow)
                {
                    yellowList.Add(charObjectArray[i].GetComponent<Character>());
                }
            }
        }

        // Try to cast skill if red force is 3 time more than own force
        if (redList.Count > 3 * yellowList.Count)
        {
            // Calculate average position of all opponent character
            float avrZ = 0f;
            foreach (Character ch in redList)
            {
                avrZ += ch.transform.position.z;
            }
            avrZ /= redList.Count;
            if (avrZ > skillRangeZMin && avrZ < skillRangeZMax)
            {
                skillSpawner.spawn(new Vector3(0f, 0f, avrZ));
            }
        }

        yield return new WaitForSeconds(15f);
        isCastingSkill = false;
    }
}
