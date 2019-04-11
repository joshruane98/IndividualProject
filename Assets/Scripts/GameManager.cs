﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInst;
    PlayerController playerInstance;
    BattleManager battleManager;

    IDictionary<string, int> enemyStats;
    Vector3 playerLastOverworldPosition;

    // Start is called before the first frame update
    void Awake()
    {
        SetupGM();
        playerInstance = PlayerController.onlyPlayerController; //Store reference to instance of player.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    void SetupGM()
    {
        //Check if there is already an Instance of Game Manager
        if (gameManagerInst != null)
        {
            Destroy(this.gameObject);
        }
        gameManagerInst = this;
        //Ensure Game Manager persists through all scenes
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void LoadBattle(IDictionary<string, int> _enemyStats)
    {
        playerLastOverworldPosition = playerInstance.gameObject.transform.position;
        Debug.Log(playerLastOverworldPosition);
        SceneManager.LoadScene(2);
        enemyStats = _enemyStats;
        /*
        SceneManager.sceneLoaded += ;
        BattleManager battleManager = (BattleManager)GameObject.Find("BattleManagerObject").GetComponent(typeof(BattleManager));
        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            if (battleManager != null)
            {
                Debug.Log("Theres a battle manager");
            }
        }
        battleManager.assignEnemyStats(enemyStats);
        */
    }

    public IDictionary<string, int> SendEnemyStats()
    {
        return enemyStats;
    }

    public void LoadOverwold()
    {
        enemyStats = null;
        playerInstance.gameObject.transform.position = playerLastOverworldPosition - new Vector3(0, 0, 3);
        SceneManager.LoadScene(1);
    }

    public void freezeGameWorld()
    {
        playerInstance.movementDisabled = true;
    }

    public void unfreezeGameWorld()
    {
        playerInstance.movementDisabled = false;
    }
}
