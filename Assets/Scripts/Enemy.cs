using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int defence;
    [SerializeField] private int reflex;
    [SerializeField] private int bravery;
    [SerializeField] private int intelligence; //Used to determine chance of selecting the most sensible option in battle.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //RETRIVE STATS
    int getHealth()
    {
        return health;
    }

    int getAttack()
    {
        return attack;
    }

    int getDefence()
    {
        return defence;
    }

    int getReflex()
    {
        return reflex;
    }

    int getBravery()
    {
        return bravery;
    }

    int getIntelligence()
    {
        return intelligence;
    }
}
