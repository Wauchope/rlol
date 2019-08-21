using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            if (value < currentHealth)
            {
                maxHealth = value;
                currentHealth = value;
            }
            else if (value < 0)
            {
                Debug.Log("Tried to set MaxHealth < 0");
            }
            else
            {
                float percent = GetHealthPercentAsFloat();
                maxHealth = value;
                currentHealth = value * (int) percent;
            }
        }
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            if (value > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (value <= 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }

            //UpdateBar();
        }
    }

    /*
    public Health(int currentHealth, int maxHealth, bool renderBarInWorld)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;

        this.renderBarInWorld = renderBarInWorld;
    }*/

    private void Start()
    {
        
    }

    void Update()
    {
        //PositionBar();
    }

    private float GetHealthPercentAsFloat()
    {
        return currentHealth / maxHealth;
    }

    public void SetCurrentHealth(int currentHP)
    {
        CurrentHealth = currentHP;
    }

    public void SetCurrentHealthPercentage(float percentage)
    {
        CurrentHealth = (int) (MaxHealth * (percentage / 100));
    }

    public void TakeDamage (int damage)
    {
        //Fancy maths stuff
        //Armour? Resistance? Magic? Who knows
        CurrentHealth -= damage;
    }
}
