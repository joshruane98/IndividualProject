using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum States
    {
        PLAYERS_TURN,
        ENEMYS_TURN,
        PLAYER_WIN,
        PLAYER_LOSE
    }

    private States currentState;

    int turnNumber;

    public Enemy enemy;
    string enemyAction;
    public PlayerController player;
    public GameObject PlayerBattleMenu;
    public Text battleCommentary;

    // Start is called before the first frame update
    void Start()
    {
        player.inBattle = true;
        decideWhoStarts();
        turnNumber = 1;
        Debug.Log(enemy.generateDescription());
    }

    // Update is called once per frame
    void Update()
    {
        checkForBattleEnd();
        Debug.Log(currentState);
        switch (currentState)
        {
            case (States.PLAYERS_TURN):
                //checkIfStunned(player);
                if (player.isStunned == false)
                {
                    battleCommentary.text = "Player's turn!";
                    PlayerBattleMenu.SetActive(true);
                }
                else
                {
                    Debug.Log("You are stunned so cant act on turn " + turnNumber);
                    battleCommentary.text = "Player is stunned so misses this turn";
                    player.turnsToMiss--;
                    if (player.turnsToMiss == 0)
                    {
                        player.isStunned = false;
                    }
                    switchTurns();
                }
                break;
            case (States.ENEMYS_TURN):
                PlayerBattleMenu.SetActive(false);
                if (enemy.isStunned == false)
                {
                    enemyAction = enemy.chooseAction(player.getHealth(), player.generateDescription(), turnNumber);
                    if (enemyAction == "attack")
                    {
                        enemy.Attack(player);
                        battleCommentary.text = "The enemy attacked you!";
                        Debug.Log("Enemy Attacked!");
                    }
                    else if (enemyAction == "speedyAttack")
                    {
                        enemy.SpeedyAttack(player);
                        battleCommentary.text = "The enemy attacked you speedily!";
                        Debug.Log("Enemy Attacked Speedily!");
                    }
                    else if (enemyAction == "massiveAttack")
                    {
                        enemy.MassiveAttack(player);
                        battleCommentary.text = "The enemy attacked you massively!";
                        Debug.Log("Enemy Attacked Massively!");
                    }
                    else if (enemyAction == "stun")
                    {
                        enemy.Stun(player);
                        battleCommentary.text = "The enemy stunned you!";
                        Debug.Log("The enemy stunned you!");
                    }
                    else if (enemyAction == "intimidate")
                    {
                        enemy.Intimidate(player);
                        battleCommentary.text = "The enemy intimidated you!";
                        Debug.Log("The enemy intimidated you - your attack and defence fell");
                    }
                    else
                    {
                        Debug.Log(enemyAction);
                    }
                }
                else
                {
                    Debug.Log("Enemy is stunned so cant act on turn " + turnNumber);
                    battleCommentary.text = "Enemy is stunned so misses this turn";
                    enemy.turnsToMiss--;
                    if (enemy.turnsToMiss == 0)
                    {
                        enemy.isStunned = false;
                    }
                }
                switchTurns();
                break;
            case (States.PLAYER_WIN):
                break;
            case (States.PLAYER_LOSE):
                break;
        }
    }

    void decideWhoStarts()
    {
        if (player.getReflex() > enemy.getReflex())
        {
            currentState = States.PLAYERS_TURN;
        }else if (player.getReflex() == enemy.getReflex())
        {
            if ((Random.Range(0, 10) % 2) == 0)
            {
                currentState = States.PLAYERS_TURN;
            }
            else
            {
                currentState = States.ENEMYS_TURN;
            }
        }else
        {
            currentState = States.ENEMYS_TURN;
        }
    }

    void switchTurns()
    {
        battleCommentary.text = "";
        if (currentState == States.PLAYERS_TURN)
        {
            currentState = States.ENEMYS_TURN;
        }
        else if (currentState == States.ENEMYS_TURN)
        {
            currentState = States.PLAYERS_TURN;
        }
        turnNumber++;
        Debug.Log("Turn " + turnNumber);
    }

    public void handlePlayerAction(string action)
    {
        Debug.Log(action);
        if (action == "Attack")
        {
            player.Attack(enemy);
            battleCommentary.text = "You attacked the enemy!";
        }
        else if (action == "SpeedyAttack")
        {
            player.SpeedyAttack(enemy);
            battleCommentary.text = "You attacked the enemy speedily!";
        }
        else if (action == "Stun")
        {
            player.Stun(enemy);
            battleCommentary.text = "You stunned the enemy!";
        }
        else if (action == "Intimidate")
        {
            player.Intimidate(enemy);
            battleCommentary.text = "You intimidated the enemy!";
        }
        switchTurns();
    }

    void checkIfStunned(BattleCharacter battleCharacter)
    {
        if (battleCharacter.isStunned)
        {
            Debug.Log("Stunned so cant act");
            battleCommentary.text = "Stunned so misses a turn!";
            switchTurns();
        }
    }
    void checkForBattleEnd()
    {
        if (player.getHealth() <= 0 )
        {
            currentState = States.PLAYER_LOSE;
        }
        else if (enemy.getHealth() <= 0 )
        {
            currentState = States.PLAYER_WIN;
        }
    }
}
