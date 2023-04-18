using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatus
{
    public enum CharacterClassE
    {
        Gunner = 0,
        Grenadier,
        Sniper,
        Rocket,
        Armour,
        Tank
    }

    public enum BuyStatusE
    {
        Success,
        Fail
    }
    public enum SideE
    {
        Yellow,
        Red
    }

    public enum WarStatus
    {
        Running,
        Lose,
        Win
    }
}
