using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MassiveAttack(BattleCharacter target)
    {

    }

    public string chooseAction(int playerHealth, int turnNumber)
    {
        IDictionary<string, float> actionWeights = new Dictionary<string, float>()
                                            {
                                                {"attack",0.0f},
                                                {"speedyAttack", 0.0f},
                                                {"massiveAttack", 0.0f},
                                                {"stun", 0.0f},
                                                {"intimidate", 0.0f}
                                            };
        Debug.Log("Attack weight before: " + actionWeights["attack"]);
        actionWeights = assignActionWeights(actionWeights, playerHealth, turnNumber);
        Debug.Log("Attack weight after: " + actionWeights["attack"]);

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

    IDictionary<string, float> assignActionWeights(IDictionary<string, float> _actionWeights, int _playerHealth, int _turnNumber)
    {
        _actionWeights["attack"] = 0.5f;
        return _actionWeights;
    }
}
