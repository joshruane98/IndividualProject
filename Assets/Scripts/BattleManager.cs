using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private bool playerTurn;
    public Enemy enemy;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        decideWhoStarts();
        if (playerTurn)
        {
            Debug.Log("It's the player's turn");
        }else
        {
            Debug.Log("It's the enemy's turn");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void decideWhoStarts()
    {
        if (player.getReflex() > enemy.getReflex())
        {
            playerTurn = true;
        }else if (player.getReflex() == enemy.getReflex())
        {
            if ((Random.Range(0, 10) % 2) == 0)
            {
                playerTurn = true;
            }else
            {
                playerTurn = false;
            }
        }else
        {
            playerTurn = false;
        }
    }
}
