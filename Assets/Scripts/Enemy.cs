using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        isStunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MassiveAttack(BattleCharacter target)
    {
        target.LoseHealth((int)(getAttackPower() * 3));
        //Enemy is exhausted from attack so misses next turn
        isStunned = true;
        turnsToMiss = 1;
    }

    public string chooseAction(int playerHealth, string playerDescription, int turnNumber)
    {
        IDictionary<string, float> actionWeights = new Dictionary<string, float>()
                                            {
                                                {"attack",0.0f},
                                                {"speedyAttack", 0.0f},
                                                {"massiveAttack", 0.0f},
                                                {"stun", 0.0f},
                                                {"intimidate", 0.0f}
                                            };
        //Debug.Log("Attack weight before: " + actionWeights["attack"]);
        actionWeights = assignActionWeights(actionWeights, playerHealth, playerDescription, turnNumber);
        Debug.Log("Attack weight: " + actionWeights["attack"]);
        Debug.Log("Speedy Attack weight: " + actionWeights["speedyAttack"]);
        Debug.Log("Massive Attack weight: " + actionWeights["massiveAttack"]);
        Debug.Log("Stun weight: " + actionWeights["stun"]);
        Debug.Log("Intimidate weight: " + actionWeights["intimidate"]);

        float rand = Random.Range(0.0f, 1.0f);

        foreach (KeyValuePair<string, float> item in actionWeights)
        {
            //Key is action, Value is weight
            if (rand < item.Value)
            {
                return item.Key;
            }
            rand -= item.Value;
        }
        return "OUT OF RANGE";//If no action is returned - shouldn't reach here
    }

    IDictionary<string, float> assignActionWeights(IDictionary<string, float> _actionWeights, int _playerHealth, string _playerDescription, int _turnNumber)
    {
        float remainingWeight;
        if (_turnNumber <= 2) //Early in battle - try to reduce player stats to make them easier to fight.
        {
            if (_playerDescription.Contains("petrified") || _playerDescription.Contains("very frightened") && getBravery() >= 33 || _playerDescription.Contains("pretty frightened") && getBravery() >= 63)
            {
                _actionWeights["intimidate"] = 0.75f;
            }
            else
            {
                _actionWeights["intimidate"] = 0.3f;
            }
            remainingWeight = 1 - _actionWeights["intimidate"];
            //Divide remaining weight between other actions
            _actionWeights["attack"] = remainingWeight * 0.65f;
            _actionWeights["speedyAttack"] = remainingWeight * 0.2f;
            _actionWeights["massiveAttack"] = remainingWeight * 0.05f;
            _actionWeights["stun"] = remainingWeight * 0.1f;
        }
        else
        {
            _actionWeights["attack"] = 0.65f;
            _actionWeights["speedyAttack"] = 0.1f;
            _actionWeights["massiveAttack"] = 0.05f;
            _actionWeights["stun"] = 0.1f;
            _actionWeights["intimidate"] = 0.1f;
        }
        return _actionWeights;
    }
}
