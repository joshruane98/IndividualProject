using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInst;
    PlayerController playerInstance;
    BattleManager battleManager;
    Canvas overworldUI;

    bool firstOverworldLoad;

    IDictionary<string, int> enemyStats;
    Vector3 playerLastOverworldPosition;
    GameObject[] enemies;
    List<int> defeatedEnemiesIDs;
    int enemyEncounteredByPlayer; //Stores ID of last enemy encountered.
    bool playerWonLastBattle;

    // Start is called before the first frame update
    void Awake()
    {
        SetupGM();
        playerInstance = PlayerController.onlyPlayerController; //Store reference to instance of player.
        overworldUI = (Canvas)GameObject.Find("OverworldUI").GetComponent(typeof(Canvas));
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            firstOverworldLoad = true;
            SceneManager.LoadScene(1);
        }
        defeatedEnemiesIDs = new List<int>();
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

    public void LoadBattle(Enemy _enemy)
    {
        enemyStats = new Dictionary<string, int>()
                                            {
                                                {"maxHealth", 0},
                                                { "health", 0},
                                                {"attack", 0},
                                                {"defence", 0},
                                                {"bravery", 0},
                                                {"reflex", 0}
                                            };
        enemyStats["maxHealth"] = _enemy.getMaxHealth();
        enemyStats["health"] = _enemy.getHealth();
        enemyStats["attack"] = _enemy.getAttackPower();
        enemyStats["defence"] = _enemy.getDefence();
        enemyStats["bravery"] = _enemy.getBravery();
        enemyStats["reflex"] = _enemy.getReflex();

        enemyEncounteredByPlayer = _enemy.id;

        playerLastOverworldPosition = playerInstance.gameObject.transform.position;
        Debug.Log(playerLastOverworldPosition);
        overworldUI.gameObject.SetActive(false);
        SceneManager.LoadScene(2);
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

    public void LoadOverwoldAfterBattle()
    {
        playerInstance.gameObject.transform.position = playerLastOverworldPosition - new Vector3(0, 0, 3);
        SceneManager.LoadScene(1);
        overworldUI.gameObject.SetActive(true);
        if (playerWonLastBattle)
        {
            defeatedEnemiesIDs.Add(enemyEncounteredByPlayer);
        }
        enemyStats = null;
        //enemyEncounteredByPlayer = null;
    }

    public void freezeGameWorld()
    {
        playerInstance.movementDisabled = true;
    }

    public void unfreezeGameWorld()
    {
        playerInstance.movementDisabled = false;
    }

    public void setPlayerWonLastBattle(bool result)
    {
        playerWonLastBattle = result;
    }

    //SCEN LOADED MANAGEMENT
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);

        if (scene.name == "Overworld")
        {
            if (firstOverworldLoad)
            {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
            }

            foreach (GameObject enemy in enemies)
            {
                Enemy _enemy = (Enemy)enemy.gameObject.GetComponent(typeof(Enemy));
                if (defeatedEnemiesIDs.Contains(_enemy.id))
                {
                    _enemy.gameObject.SetActive(false);
                }
            }
        }
    }
}
