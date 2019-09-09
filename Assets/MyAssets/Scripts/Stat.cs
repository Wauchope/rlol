using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Perhaps use inheritance here
public enum Stats
{
    Strength, Dexterity, Resilience
}

[System.Serializable]
public class Stat
{
    private int level;

    private int base_value;
    private int bonus_value = 0;
    private float base_gain_per_level;

    public Stat(Stats type, int level = 1, int base_value = 1, float base_gain = 1f)
    {
        this.level = level;
        this.base_value = base_value;
        base_gain_per_level = base_gain;
    }

    public void LevelUp()
    {
        level += 1;
        //Play particle effect
        //Play sound effect
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetValue()
    {
        return base_value + bonus_value + (int) (base_gain_per_level * level);
    }

    public int GetBonusValue()
    {
        return bonus_value;
    }

    public int GetGainPerLevel()
    {
        return (int) base_gain_per_level;
    }
    //get stat value
}
