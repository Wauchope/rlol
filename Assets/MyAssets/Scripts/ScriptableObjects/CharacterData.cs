using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    PLAYER, ENEMY
}

public abstract class CharacterData : ScriptableObject
{
    [SerializeField] protected UnitType unitType;

    [SerializeField] protected int speed;
    [SerializeField] protected int jumpForce;
    [SerializeField] protected int turnRate;

    public int Speed
    {
        get
        {
            return speed;
        }
    }

    public int JumpForce
    {
        get
        {
            return jumpForce;
        }
    }

    public int TurnRate
    {
        get
        {
            return turnRate;
        }
    }

    public UnitType UnitType
    {
        get
        {
            return unitType;
        }
    }
}
