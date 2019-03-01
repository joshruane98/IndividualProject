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
        string battleState = getStateOfBattle(playerHealth, turnNumber);

        IDictionary<string, float> actionWeights = new Dictionary<string, float>()
                                            {
                                                {"attack",0.25f},
                                                {"speedyAttack", 0.75f}
                                            };


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

    string getStateOfBattle(int playerHealth, int numberOfTurns)
    {
        return "";
    }

    
}
