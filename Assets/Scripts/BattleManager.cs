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

    // Start is called before the first frame update
    void Start()
    {
        decideWhoStarts();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case (States.PLAYERS_TURN):
                break;
            case (States.ENEMYS_TURN):
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

    void dealAttackOn(PlayerController _player)
    {

    }

    void dealAttackOn(Enemy _enemy)
    {

    }
}
