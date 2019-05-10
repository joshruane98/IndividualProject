using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class: Enemy
//The class responsible for all enemy behaviour. Extends the Battle Character Class.
public class Enemy : BattleCharacter
{
    //Variable: speedyAttackFailed
    //Identifies if a Sppedy Attack has previously failed. If it has then this will not be chosen as a preferred action again.
    bool speedyAttackFailed;

    public int id;

    //Debug UI to display weights
    public Text attackWeightDisp;
    public Text spAttackWeightDisp;
    public Text mAttackWeightDisp;
    public Text stunWeightDisp;
    public Text intimiWeightDisp;

    //Function: Awake
    //Unity function with unique behaviour. Set-up function used for initialisation.
    void Awake()
    {
        isStunned = false;
        bool speedyAttackFailed = false;
        anim = GetComponent<Animator>();
    }

    /* Function: MassiveAttack

        Executes a Massive Attack on the player, dealing triple the normal amount of damage. Stuns the enemy for one turn after use.

       Parameters:

          target - The target for the attack (the player).

    */
    public void MassiveAttack(BattleCharacter target)
    {
        float attackPwr = getAttackPower();
        int damage = (int)(attackPwr - (attackPwr * ((float)target.getDefence() / 200.0f))); //200 because 200 is max value.
        target.LoseHealth((int)(damage * 3));
        //Enemy is exhausted from attack so misses next turn
        isStunned = true;
        source.PlayOneShot(attackSound);
        anim.Play("Attack");
        turnsToMiss = 1;
    }

    /* Function: chooseAction

        Used to randomly select an attack using the weights assigned in assignActionWeights().
        Called by the BattleManager class on the enemy's turn.

       Parameters:

          playerHealth - The player's current health.
          playerDescription - The description of the player, suggesting its stats.
          turnNumber - The current turn nuber in the battle.
       
       Returns:

          The chosen action.

    */
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

    /* Function: chooseAction

        Used to assign weightings to each action based on the current situation. Implements the decision tree designed to represent the enemy's AI.
        Called by chooseAction().

       Parameters:

          _actionWeights - The Dictionary containing key-value pairs made up of the actions and their weights, initialised as 0, ready to be assigned.
          _playerHealth - The player's current health.
          _playerDescription - The description of the player, suggesting its stats.
          _turnNumber - The current turn nuber in the battle.

       Returns:

          The Dictionary of Action Weights, with appropriately assigned weights of each action dependent on the current battle situation.

    */
    IDictionary<string, float> assignActionWeights(IDictionary<string, float> _actionWeights, int _playerHealth, string _playerDescription, int _turnNumber)
    {
        float remainingWeight;
        /*
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
            return _actionWeights;
        }
        else if (getHealth() <= getMaxHealth() * 0.2)
        {
            _actionWeights["attack"] = 0.15f;
            _actionWeights["speedyAttack"] = 0.15f;
            _actionWeights["massiveAttack"] = 0.65f;
            _actionWeights["stun"] = 0.025f;
            _actionWeights["intimidate"] = 0.025f;
            return _actionWeights;
        }
        else
        {
            _actionWeights["attack"] = 0.65f;
            _actionWeights["speedyAttack"] = 0.1f;
            _actionWeights["massiveAttack"] = 0.05f;
            _actionWeights["stun"] = 0.1f;
            _actionWeights["intimidate"] = 0.1f;
            return _actionWeights;
        }
        */
        //NEW IMPLEMENTATION
        if (_playerDescription.Contains("petrified")) {
            if (_turnNumber <= 2)
            {
                _actionWeights["intimidate"] = 0.75f;
                remainingWeight = 1 - _actionWeights["intimidate"];
                //Divide remaining weight between other actions
                _actionWeights["attack"] = remainingWeight * 0.65f;
                _actionWeights["speedyAttack"] = remainingWeight * 0.2f;
                _actionWeights["massiveAttack"] = remainingWeight * 0.05f;
                _actionWeights["stun"] = remainingWeight * 0.1f;
                displayWeightings(_actionWeights);
                return _actionWeights;
            }
        }

        if (getHealth() < (getMaxHealth() * 0.2))
        {
            _actionWeights["massiveAttack"] = 0.65f;
            remainingWeight = 1 - _actionWeights["massiveAttack"];
            //Divide remaining weight between other actions
            _actionWeights["attack"] = remainingWeight * 0.65f;
            _actionWeights["speedyAttack"] = remainingWeight * 0.2f;
            _actionWeights["intimidate"] = remainingWeight * 0.05f;
            _actionWeights["stun"] = remainingWeight * 0.1f;
            displayWeightings(_actionWeights);
            return _actionWeights;
        }

        if (getHealth() < (getMaxHealth() * 0.5))
        {
            _actionWeights["massiveAttack"] = 0.4f;
            remainingWeight = 1 - _actionWeights["massiveAttack"];
            //Divide remaining weight between other actions
            _actionWeights["attack"] = remainingWeight * 0.65f;
            _actionWeights["speedyAttack"] = remainingWeight * 0.2f;
            _actionWeights["intimidate"] = remainingWeight * 0.05f;
            _actionWeights["stun"] = remainingWeight * 0.1f;
            displayWeightings(_actionWeights);
            return _actionWeights;
        }

        if (_playerDescription.Contains("extremely slow") && speedyAttackFailed != true)
        {
            _actionWeights["speedyAttack"] = 0.6f;
            remainingWeight = 1 - _actionWeights["speedyAttack"];
            //Divide remaining weight between other actions
            _actionWeights["attack"] = remainingWeight * 0.65f;
            _actionWeights["massiveAttack"] = remainingWeight * 0.2f;
            _actionWeights["intimidate"] = remainingWeight * 0.05f;
            _actionWeights["stun"] = remainingWeight * 0.1f;
            displayWeightings(_actionWeights);
            return _actionWeights;
        }
        else if (_playerDescription.Contains("very slow") && speedyAttackFailed != true)
        {
            _actionWeights["speedyAttack"] = 0.4f;
            remainingWeight = 1 - _actionWeights["speedyAttack"];
            //Divide remaining weight between other actions
            _actionWeights["attack"] = remainingWeight * 0.65f;
            _actionWeights["massiveAttack"] = remainingWeight * 0.2f;
            _actionWeights["intimidate"] = remainingWeight * 0.05f;
            _actionWeights["stun"] = remainingWeight * 0.1f;
            displayWeightings(_actionWeights);
            return _actionWeights;
        }
        else if (_playerDescription.Contains("pretty slow") && speedyAttackFailed != true)
        {
            _actionWeights["speedyAttack"] = 0.3f;
            remainingWeight = 1 - _actionWeights["speedyAttack"];
            //Divide remaining weight between other actions
            _actionWeights["attack"] = remainingWeight * 0.65f;
            _actionWeights["massiveAttack"] = remainingWeight * 0.2f;
            _actionWeights["intimidate"] = remainingWeight * 0.05f;
            _actionWeights["stun"] = remainingWeight * 0.1f;
            displayWeightings(_actionWeights);
            return _actionWeights;
        }

        //Default weightings to use
        _actionWeights["attack"] = 0.65f;
        _actionWeights["speedyAttack"] = 0.1f;
        _actionWeights["massiveAttack"] = 0.05f;
        _actionWeights["stun"] = 0.1f;
        _actionWeights["intimidate"] = 0.1f;
        displayWeightings(_actionWeights);
        return _actionWeights;
    }

    //Function: displayWeightings
    //Debug funtion to display all weights on the battle UI to aid in assessment.
    void displayWeightings(IDictionary<string, float> _actionWeights)
    {
        attackWeightDisp.text = "Attack weight: " + _actionWeights["attack"];
        spAttackWeightDisp.text = "Speedy Attack weight: " + _actionWeights["speedyAttack"];
        mAttackWeightDisp.text = "Massive Attack weight: " + _actionWeights["massiveAttack"];
        stunWeightDisp.text = "Stun weight: " + _actionWeights["stun"];
        intimiWeightDisp.text = "Intimidate weight: " + _actionWeights["intimidate"];
}
}
