using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Class: BattleManager
//The class that controls the entire turn-based battle.
public class BattleManager : MonoBehaviour
{
    //REFERENCES TO BATTLE CHARACTERS
    //Variable: player
    //Reference to the player.
    public PlayerController player;
    //Variable: enemy
    //Reference to the enemy.
    public Enemy enemy;

    //Variable: gameManager
    //Reference to the Game Manager
    GameManager gameManager;

    //STATE VARIABLES
    //Variable: States
    //Defines the different states of the battle: Player Turn, Enemy Turn, Wait Between Turns, Player Win, Player Lose.
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

    //Variable: enemyAction
    //The action the enemy has selected to take.
    string enemyAction;

    //UI
    public GameObject PlayerBattleMenu;
    public Text battleCommentary;
    public Text enemyHealthUI;
    private Animator enemyHealthUIAnim;
    public Text playerHealthUI;
    private Animator playerHealthUIAnim;
    public Text enemyDescriptionUI_TEMP;
    public GameObject blackBG;
    private Animator blackBGAnim;
    public GameObject gameOverText;
    private Animator gameOverTextAnim;
    //Debug to display stats for illustrative purposes
    public Text PAttackDisp;
    private Animator PAttackDispAnim;
    public Text PDefenceDisp;
    private Animator PDefenceDispAnim;
    public Text PBraveryDisp;
    public Text PReflexDisp;
    public Text EAttackDisp;
    private Animator EAttackDispAnim;
    public Text EDefencekDisp;
    private Animator EDefenceDispAnim;
    public Text EBraveryDisp;
    public Text EReflexDisp;



    //Function: Start
    //Unity function with unique behaviour. Set-up function. Calls assignEnemyStats(). Calls decideWhoStarts(). General set up of references, UI and animators.
    void Start()
    {
        gameManager = GameManager.gameManagerInst;

        assignEnemyStats();

        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        player.movementDisabled = true;
        player.setBattlePosition();
        decideWhoStarts();
        turnNumber = 0;
        Debug.Log(enemy.generateDescription());
        enemyDescriptionUI_TEMP.text = enemy.generateDescription();
        playerHealthUI.text = player.getHealth().ToString();
        enemyHealthUI.text = enemy.getHealth().ToString();

        enemyHealthUIAnim = (Animator)enemyHealthUI.GetComponent(typeof(Animator));
        EAttackDispAnim = (Animator)EAttackDisp.GetComponent(typeof(Animator));
        EDefenceDispAnim = (Animator)EDefencekDisp.GetComponent(typeof(Animator));
        playerHealthUIAnim = (Animator)playerHealthUI.GetComponent(typeof(Animator));
        PAttackDispAnim = (Animator)PAttackDisp.GetComponent(typeof(Animator));
        PDefenceDispAnim = (Animator)PDefenceDisp.GetComponent(typeof(Animator));
        blackBGAnim = (Animator)blackBG.GetComponent(typeof(Animator));
        gameOverTextAnim = (Animator)gameOverText.GetComponent(typeof(Animator));
    }

    /*Function: Update
     
     Unity function with unique behaviour. Update is called once per frame. Calls checkForBattleEnd. Updates UI. Contains switch statement for States.
     Player Turn:
        Checks if player is stunned. If player is stunned, turns to miss is decremented.
        PlayerBattleMenu is made active to allow player to choose an action.
    Enemy Turn:
        Checks if enemy is stunned. If enemy is stunned, turns to miss is decremented.
        Setes enemyAction to the return value of enemy.chooseAction(), passing player health, player description and turn number as parameters.
        Handles enemyAction using if statement to check what enemyAction is, then call the coorect function, e.g. enemy.Attack().
    Player Win:
        Sets gameManager.setPlayerWonLastBattle() to true.
    Player Lose:
        Sets gameManager.setPlayerWonLastBattle() to false.
        Activates Game Over Screen.

    endTurn() last function called in every case.

    */
    void Update()
    {
        checkForBattleEnd();
        displayStatsUI();
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
                        playerHealthUIAnim.Play("UIDecrease");
                    }
                    else if (enemyAction == "speedyAttack")
                    {
                        bool speedyAttackSucessful = enemy.SpeedyAttack(player);
                        if (speedyAttackSucessful)
                        {
                            battleCommentary.text = "The enemy attacked you speedily!";
                            playerHealthUIAnim.Play("UIDecrease");
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
                        playerHealthUIAnim.Play("UIDecrease");
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
                            PAttackDispAnim.Play("UIDecrease");
                            PDefenceDispAnim.Play("UIDecrease");
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
                battleCommentary.text = "You defeated the enemy and won the battle!";
                gameManager.setPlayerWonLastBattle(true);
                endTurn();
                break;
            case (States.PLAYER_LOSE):
                blackBG.SetActive(true);
                blackBGAnim.SetBool("gameOver", true);
                gameOverText.SetActive(true);
                gameOverTextAnim.SetBool("gameOver", true);
                battleCommentary.text = "The enemy defeated you! You lost the battle.";
                gameManager.setPlayerWonLastBattle(false);
                endTurn();
                break;
        }
    }

    /*Function: decideWhoStarts

     Compares player and enemy reflex stats to determine who starts the battle.
     If they are the same, a random number is generated. If it's even, player starts, if it's odd, enemy starts.
     Sets currentState to PLAYERS_TURN or ENEMYS_TURN respectively.

    */
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

    /*Function: assignEnemyStats

     Calls gameManager.SendEnemyStats() to retrieve dictionary containing enemy stats from enemy encountered in overworld.
     These stats are assigned to the instance of thre enemy in the battle, using the relevant setter functions.

    */
    public void assignEnemyStats()
    {
        IDictionary<string, int> enemyStats = gameManager.SendEnemyStats();
        enemy.setMaxHealth(enemyStats["maxHealth"]);
        enemy.setHealth(enemyStats["health"]);
        enemy.setAttackPower(enemyStats["attack"]);
        enemy.setDefence(enemyStats["defence"]);
        enemy.setBravery(enemyStats["bravery"]);
        enemy.setReflex(enemyStats["reflex"]);
    }

    void displayStatsUI()
    {
        PAttackDisp.text = "Attack: " + player.getAttackPower();
        PDefenceDisp.text = "Defence: " + player.getDefence();
        PBraveryDisp.text = "Bravery: " + player.getBravery();
        PReflexDisp.text = "Reflex: " + player.getReflex();

        EAttackDisp.text = "Attack: " + enemy.getAttackPower();
        EDefencekDisp.text = "Defence: " + enemy.getDefence();
        EBraveryDisp.text = "Bravery: " + enemy.getBravery();
        EReflexDisp.text = "Reflex: " + enemy.getReflex();
}

    /*Function: endTurn

     If the current its curently the players turn, PlayerBattleMenu is deactivated.
     previousState set to current state. Current state set to WAIT_BETWEEN_TURNS.
     Starts Wait() coroutine.

    */
    void endTurn()
    {
        playerHealthUI.text = player.getHealth().ToString();
        enemyHealthUI.text = enemy.getHealth().ToString();
        if (currentState == States.PLAYERS_TURN)
        {
            PlayerBattleMenu.SetActive(false);
        }
        previousState = currentState;
        currentState = States.WAIT_BETWEEN_TURNS;
        StartCoroutine(Wait());
    }

    /*Function: switchTurns

     Switches from players turn to enemy's turn and vice-versa.
     Increments turnNumber.

    */
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

    /* Function: handlePlayerAction

        Called when the player clicks one of the buttons in PlayerActionMenu.
        Handles players action using if statement to check what action is, then call the correct function, e.g. player.Attack(). 

       Parameters:

          action - The action selected by the player.

    */
    public void handlePlayerAction(string action)
    {
        Debug.Log(action);
        if (action == "Attack")
        {
            player.Attack(enemy);
            battleCommentary.text = "You attacked the enemy!";
            enemyHealthUIAnim.Play("UIDecrease");
        }
        else if (action == "SpeedyAttack")
        {
            bool speedyAttackSucessful = player.SpeedyAttack(enemy);
            if (speedyAttackSucessful)
            {
                battleCommentary.text = "You attacked the enemy speedily!";
                enemyHealthUIAnim.Play("UIDecrease");
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
                battleCommentary.text = "You intimidated the enemy - its attack and defence fell!";
                EAttackDispAnim.Play("UIDecrease");
                EDefenceDispAnim.Play("UIDecrease");
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

    /*Function: checkForBattleEnd

     Checks if either player or enemy health is zero. Sets currentState to PLAYER_WIN or PLAYER_LOSE where appropriate.

    */
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

    /*Function: Wait

     Coroutine. Waits for three secons before calling switchTurns. Also calls gameManager.restartGame() if player has lost or gameManager.LoadOverworldAfterBattle()
     if player has won, after waiting.

    */
    IEnumerator Wait()
    {
        Debug.Log("Waiting.........");

        if (previousState == States.PLAYER_LOSE)
        {
            yield return new WaitForSeconds(5);
            gameManager.restartGame();
        }
        else if (previousState == States.PLAYER_WIN)
        {
            yield return new WaitForSeconds(3);
            player.movementDisabled = false;
            Debug.Log("LEAVING BATTLE");
            gameManager.LoadOverwoldAfterBattle();
        }
        else
        {
            yield return new WaitForSeconds(3);
            switchTurns();
        }
    }
}
