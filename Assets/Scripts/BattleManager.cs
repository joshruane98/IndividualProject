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
        WAIT_BETWEEN_TURNS,
        PLAYER_WIN,
        PLAYER_LOSE
    }

    private States currentState;
    private States previousState;

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
                    endTurn();
                }
                break;
            case (States.ENEMYS_TURN):
                PlayerBattleMenu.SetActive(false);
                if (enemy.isStunned == false)
                {
                    battleCommentary.text = "Enemy's turn!";
                    enemyAction = enemy.chooseAction(player.getHealth(), player.generateDescription(), turnNumber);
                    if (enemyAction == "attack")
                    {
                        enemy.Attack(player);
                        battleCommentary.text = "The enemy attacked you!";
                        Debug.Log("Enemy Attacked!");
                    }
                    else if (enemyAction == "speedyAttack")
                    {
                        bool speedyAttackSucessful = enemy.SpeedyAttack(player);
                        if (speedyAttackSucessful)
                        {
                            battleCommentary.text = "The enemy attacked you speedily!";
                        }
                        else if (!speedyAttackSucessful)
                        {
                            battleCommentary.text = "The enemy attacked you speedily...but the attack was unsucessful!";
                        }
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
                        bool intimidationSucessful = enemy.Intimidate(player);
                        if (intimidationSucessful)
                        {
                            battleCommentary.text = "The enemy intimidated you!";
                        }
                        else if (!intimidationSucessful)
                        {
                            battleCommentary.text = "The enemy couldn't intimidate you!";
                        }
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
                endTurn();
                break;
            case (States.WAIT_BETWEEN_TURNS):
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

    void endTurn()
    {
        if (currentState == States.PLAYERS_TURN)
        {
            PlayerBattleMenu.SetActive(false);
        }
        previousState = currentState;
        currentState = States.WAIT_BETWEEN_TURNS;
        StartCoroutine(Wait());
    }
    
    void switchTurns()
    {
        battleCommentary.text = "";
        if (previousState == States.PLAYERS_TURN)
        {
            currentState = States.ENEMYS_TURN;
        }
        else if (previousState == States.ENEMYS_TURN)
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
            bool speedyAttackSucessful = player.SpeedyAttack(enemy);
            if (speedyAttackSucessful)
            {
                battleCommentary.text = "You attacked the enemy speedily!";
            }
            else if (!speedyAttackSucessful)
            {
                battleCommentary.text = "You attacked the enemy speedily...but the attack failed!";
            }
        }
        else if (action == "Stun")
        {
            player.Stun(enemy);
            battleCommentary.text = "You stunned the enemy!";
        }
        else if (action == "Intimidate")
        {
            bool intimidationSucessful = player.Intimidate(enemy);
            if (intimidationSucessful)
            {
                battleCommentary.text = "You intimidated the enemy!";
            }
            else if (!intimidationSucessful)
            {
                battleCommentary.text = "You couldn't intimidate the enemy!";
            }
            
        }
        endTurn();
    }

    void checkIfStunned(BattleCharacter battleCharacter)
    {
        if (battleCharacter.isStunned)
        {
            Debug.Log("Stunned so cant act");
            battleCommentary.text = "Stunned so misses a turn!";
            endTurn();
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

    IEnumerator Wait()
    {
        Debug.Log("Waiting.........");
        yield return new WaitForSeconds(3);
        switchTurns();
    }
}
