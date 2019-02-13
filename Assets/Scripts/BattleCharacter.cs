using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;
    [SerializeField] protected int baseAttackPower; //basic attack power
    [SerializeField] protected int attackPower; //current attack power including any effects
    [SerializeField] protected int baseDefence; //basic defence stat
    [SerializeField] protected int defence; //current defence including any effects
    [SerializeField] protected int bravery;
    [SerializeField] protected int reflex;

    //---RETRIVE STATS---
    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public int getBaseAttackPower()
    {
        return baseAttackPower;
    }

    public int getAttackPower()
    {
        return attackPower;
    }

    public int getBaseDefence()
    {
        return baseDefence;
    }

    public int getDefence()
    {
        return defence;
    }

    public int getBravery()
    {
        return bravery;
    }

    public int getReflex()
    {
        return reflex;
    }

    //---ALTER STATS---

    //Health
    public void gainHealth(int amount)
    {
        health += amount;
    }

    public void loseHealth(int amount)
    {
        health -= amount;
    }

    //Attack
    public void increaseAttackPower(int amount)
    {
        attackPower += amount;
    }

    public void decreaseAttackPower(int amount)
    {
        attackPower -= amount;
    }

    public void resetAttackPower()
    {
        attackPower = getBaseAttackPower();
    }

    //Defence
    public void increaseDefence(int amount)
    {
        defence += amount;
    }

    public void decreaseDefence(int amount)
    {
        defence -= amount;
    }

    public void resetDefence()
    {
        defence = getBaseDefence();
    }
}
