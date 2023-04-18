using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public GameSpawner redSpawner;
    public GameSpawner yellowSpawner;

    private GameStatus.WarStatus warStat = GameStatus.WarStatus.Running;

    public GameStatus.WarStatus checkWarStatus()
    {
        return warStat;
    }

    public void pause()
    {
        Time.timeScale = 0f;
    }

    public void resume()
    {
        Time.timeScale = 1f;
    }

    IEnumerator winCoroutine()
    {
        yield return new WaitForSeconds(1f);
        warStat = GameStatus.WarStatus.Win;
    }

    IEnumerator loseCoroutine()
    {
        yield return new WaitForSeconds(1f);
        warStat = GameStatus.WarStatus.Lose;
    }
}
