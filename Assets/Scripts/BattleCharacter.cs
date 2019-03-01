using System;
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
    protected bool isStunned;

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
    public void GainHealth(int amount)
    {
        health += amount;
    }

    public void LoseHealth(int amount)
    {
        health -= amount;
    }

    //Attack
    public void IncreaseAttackPower(int amount)
    {
        attackPower += amount;
    }

    public void DecreaseAttackPower(int amount)
    {
        attackPower -= amount;
    }

    public void ResetAttackPower()
    {
        attackPower = getBaseAttackPower();
    }

    //Defence
    public void IncreaseDefence(int amount)
    {
        defence += amount;
    }

    public void DecreaseDefence(int amount)
    {
        defence -= amount;
    }

    public void ResetDefence()
    {
        defence = getBaseDefence();
    }

    //---BATTLE ACTIONS---
    public void Attack(BattleCharacter target)
    {
        target.LoseHealth(getAttackPower());
    }

    public void SpeedyAttack(BattleCharacter target)
    {
        target.LoseHealth((int)(getAttackPower() * 1.5));
    }

    public void Intimidate(BattleCharacter target)
    {

    }

    public void Stun(BattleCharacter target)
    {

    }

    public void Observe(BattleCharacter target)
    {

    }
}
