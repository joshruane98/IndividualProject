//using System;
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

    public ParticleSystem hurtEffect;
    public ParticleSystem intimidatedEffect;

    public bool isStunned; //To miss turns in battle
    public int turnsToMiss; //How many turns to miss when stunned

    protected Animator anim;
    //---SET STATS---
    public void setMaxHealth(int amount)
    {
        maxHealth = amount;
    }

    public void setHealth(int amount)
    {
        health = amount;
    }

    public void setAttackPower(int amount)
    {
        attackPower = amount;
    }

    public void setDefence(int amount)
    {
        defence = amount;
    }

    public void setBravery(int amount)
    {
        bravery = amount;
    }

    public void setReflex(int amount)
    {
        reflex = amount;
    }
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
        if (health < 0)
        {
            health = 0;
        }
        hurtEffect.Play();
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
        intimidatedEffect.Play();
    }

    public void ResetDefence()
    {
        defence = getBaseDefence();
    }

    //---BATTLE ACTIONS---
    public void Attack(BattleCharacter target)
    {
        float attackPwr = getAttackPower();
        int damage = (int)(attackPwr - (attackPwr * ((float)target.getDefence() / 200.0f))); //200 because 200 is max value.
        Debug.Log("Damage is: " + damage);
        if (damage <= 0)
        {
            damage = 1;
        }
        target.LoseHealth(damage);
        anim.Play("Attack");
    }

    public bool SpeedyAttack(BattleCharacter target)
    {
        float attackPwr = getAttackPower();
        int damage = (int)(attackPwr - (attackPwr * ((float)target.getDefence() / 200.0f))); //200 because 200 is max value.
        if (getReflex() > (target.getReflex() + (target.getReflex() * 0.1)))
        {
            target.LoseHealth((int)(damage * 1.5));
            return true;
        }
        else
        {
            Debug.Log("Speedy attack failed");
            return false;
        }
    }

    public bool Intimidate(BattleCharacter target)
    {
        if (getBravery() > target.getBravery())
        {
            target.DecreaseAttackPower((int)(target.getAttackPower() / 4));
            target.DecreaseDefence((int)(target.getDefence() / 4));
            return true;
        }
        else if (getBravery() == target.getBravery())
        {
            if ((Random.Range(0, 10) % 2) == 0)
            {
                target.DecreaseAttackPower((int)(target.getAttackPower() / 4));
                target.DecreaseDefence((int)(target.getDefence() / 4));
                return true;
            }
            Debug.Log("Intimidation failed");
            return false;
        }
        else
        {
            Debug.Log("Intimidation failed");
            return false;
        }
    }

    public void Stun(BattleCharacter target)
    {
        target.isStunned = true;
        target.turnsToMiss = 1;
    }

    public void Observe(BattleCharacter target)
    {

    }

    public string generateDescription()
    {
        int _attack = getAttackPower();
        int _defence = getDefence();
        int _bravery = getBravery();
        int _reflex = getReflex();

        string attackDescriptor;
        string defenceDescriptor;
        string braveryDescriptor;
        string reflexDescriptor;

        string description;

        //ATTACK
        if (_attack > 0 && _attack <= 25)
        {
            attackDescriptor = "like it couldn't hurt a fly";
        }
        else if (_attack > 25 && _attack <= 50)
        {
            attackDescriptor = "like it'd struggle to hurt you";
        }
        else if (_attack > 50 && _attack <= 75)
        {
            attackDescriptor = "like it couldn't do much damage";
        }
        else if (_attack > 75 && _attack <= 100)
        {
            attackDescriptor = "like it could hurt you a bit";
        }
        else if (_attack > 100 && _attack <= 125)
        {
            attackDescriptor = "like it could do some damage";
        }
        else if (_attack > 125 && _attack <= 150)
        {
            attackDescriptor = "like it could do a fair bit of damage";
        }
        else if (_attack > 150 && _attack <= 175)
        {
            attackDescriptor = "like it could do a lot of damage";
        }
        else if (_attack > 175 && _attack <= 200)
        {
            attackDescriptor = " extremely ferocious";
        }
        else
        {
            attackDescriptor = "ERROR!!";
        }

        //DEFENCE
        if (_defence > 0 && _defence <= 25)
        {
            defenceDescriptor = "a stiff breeze would blow it over";
        }
        else if (_defence > 25 && _defence <= 50)
        {
            defenceDescriptor = "it's very weak";
        }
        else if (_defence > 50 && _defence <= 75)
        {
            defenceDescriptor = "it's quite weak";
        }
        else if (_defence > 75 && _defence <= 100)
        {
            defenceDescriptor = "it's average in strength";
        }
        else if (_defence > 100 && _defence <= 125)
        {
            defenceDescriptor = "it's quite tough";
        }
        else if (_defence > 125 && _defence <= 150)
        {
            defenceDescriptor = "it's tough";
        }
        else if (_defence > 150 && _defence <= 175)
        {
            defenceDescriptor = "it's very tough";
        }
        else if (_defence > 175 && _defence <= 200)
        {
            defenceDescriptor = "it's hard as nails";
        }
        else
        {
            defenceDescriptor = "ERROR!!";
        }

        //BRAVERY
        if (_bravery > 0 && _bravery <= 25)
        {
            braveryDescriptor = "petrified";
        }
        else if (_bravery > 25 && _bravery <= 50)
        {
            braveryDescriptor = "very frightened";
        }
        else if (_bravery > 50 && _bravery <= 75)
        {
            braveryDescriptor = "pretty frightened";
        }
        else if (_bravery > 75 && _bravery <= 100)
        {
            braveryDescriptor = "slightly nervous";
        }
        else if (_bravery > 100 && _bravery <= 125)
        {
            braveryDescriptor = "indifferent";
        }
        else if (_bravery > 125 && _bravery <= 150)
        {
            braveryDescriptor = "fairly brave";
        }
        else if (_bravery > 150 && _bravery <= 175)
        {
            braveryDescriptor = "very brave";
        }
        else if (_bravery > 175 && _bravery <= 200)
        {
            braveryDescriptor = "got nerves of steel";
        }
        else
        {
            braveryDescriptor = "ERROR!!";
        }

        //REFLEX
        if (_reflex > 0 && _reflex <= 25)
        {
            reflexDescriptor = "extremely slow";
        }
        else if (_reflex > 25 && _reflex <= 50)
        {
            reflexDescriptor = "very slow";
        }
        else if (_reflex > 50 && _reflex <= 75)
        {
            reflexDescriptor = "pretty slow";
        }
        else if (_reflex > 75 && _reflex <= 100)
        {
            reflexDescriptor = "average speed";
        }
        else if (_reflex > 100 && _reflex <= 125)
        {
            reflexDescriptor = "above average speed";
        }
        else if (_reflex > 125 && _reflex <= 150)
        {
            reflexDescriptor = "pretty quick";
        }
        else if (_reflex > 150 && _reflex <= 175)
        {
            reflexDescriptor = "very quick";
        }
        else if (_reflex > 175 && _reflex <= 200)
        {
            reflexDescriptor = "lightning quick";
        }
        else
        {
            reflexDescriptor = "ERROR!!";
        }

        //BUILD DESCRIPTION
        description = "An enemy appeared! It looks " + attackDescriptor + " and that " + defenceDescriptor + ". You can see in its eyes it's " + braveryDescriptor + " and it appears to have " + reflexDescriptor + " reflexes!";

        return description;
    }
}
