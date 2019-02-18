using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Enemy enemy;
    public PlayerController player;
    public GameObject PlayerBattleMenu;

    // Start is called before the first frame update
    void Start()
    {
        player.inBattle = true;
        decideWhoStarts();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case (States.PLAYERS_TURN):
                PlayerBattleMenu.SetActive(true);
                //switchTurns();
                break;
            case (States.ENEMYS_TURN):
                PlayerBattleMenu.SetActive(false);
                enemy.Attack(player);
                Debug.Log("Enemy Attacked!");
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
        if (currentState == States.PLAYERS_TURN)
        {
            currentState = States.ENEMYS_TURN;
        }
        else if (currentState == States.ENEMYS_TURN)
        {
            currentState = States.PLAYERS_TURN;
        }
    }

    void dealAttackOn(PlayerController _player)
    {

    }
}
