using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class: GameManager
//Handles overall game state. Persistent across all scenes.
public class GameManager : MonoBehaviour
{
    //Variable: gameManagerInst
    //Stores the instance of the Game Manager. Used for checking to ensure one instance of PlayerController persists across all scenes
    public static GameManager gameManagerInst;
    //Variable: playerInstance
    //Reference to the player
    PlayerController playerInstance;
    //Variable: battleManager
    //Reference to the battle manager stored if currently in a battle.
    BattleManager battleManager;
    Canvas overworldUI;

    public GameObject demoEndBG;
    Animator demoEndBGAnim;
    public GameObject demoEndText;
    Animator demoEndTextAnim;

    //Variable: firstOverworldLoad
    //Detects if the overworld is being loaded for the first time.
    bool firstOverworldLoad;

    //Variable: enemyStats
    //A dictionary to contain the enemy stats of the enemy the player is entering into battle with. Obtained from the player. 
    IDictionary<string, int> enemyStats;
    //Variable: playerLastOverworldPosition
    //Stores the last position of the player in the overworld in order to return them to this position after battle.
    Vector3 playerLastOverworldPosition;
    //Variable: enemies
    //All enemies in the overworld.
    GameObject[] enemies;
    //Variable: defeatedEnemiesIDs
    //Stores the ID's of all defeated enemies so that they are removed from the overworld when it is loaded.
    List<int> defeatedEnemiesIDs;
    //Variable: items
    //All items in the overworld.
    GameObject[] items;
    //Variable: collectedInventoryItemIDs
    //Stores the ID's of all collected items so that they are removed from the overworld when it is loaded.
    List<int> collectedInventoryItemIDs;
    //Variable: enemyEncounteredByPlayer
    //Stores ID of last enemy encountered.
    int enemyEncounteredByPlayer;
    //Variable: playerWonLastBattle
    //Signals if the last battle was won or not.
    bool playerWonLastBattle;
    public int numberOfEnemiesBeaten;
    public int numberOfQuestsCompleted;

    /* Function: Awake
        Unity function with unique behaviour. Set-up funtion. 
        Calls SetupGM(). Obtains the instance of the player. Checks if the currently loaded scene is the Preload Scene (as this is where
        the Game Manager is instantiated). If it is, the Overworld is loaded. Initialises variables.
    */
    void Awake()
    {
        SetupGM();
        overworldUI = (Canvas)GameObject.Find("OverworldUI").GetComponent(typeof(Canvas));
        if (SceneManager.GetActiveScene().name == "Preload")
        {
            firstOverworldLoad = true;
            SceneManager.LoadScene("Overworld");
        }
        defeatedEnemiesIDs = new List<int>();
        collectedInventoryItemIDs = new List<int>();
        numberOfEnemiesBeaten = 0;
        numberOfQuestsCompleted = 0;

        demoEndBGAnim = (Animator)demoEndBG.GetComponent(typeof(Animator));
        demoEndTextAnim = (Animator)demoEndText.GetComponent(typeof(Animator));
    }

    /* Function: SetupGM
        Checks to see if an instance of a game manager is already active. If yes, the new instance is destroyed. If no, gameManagerInst is set to this.
    */
    void SetupGM()
    {
        //Check if there is already an Instance of Game Manager
        if (gameManagerInst != null)
        {
            Destroy(this.gameObject);
        }
        gameManagerInst = this;
        if (gameManagerInst == null)
        {
            Debug.Log("Game Manager Not Instantiated");
        }
        //Ensure Game Manager persists through all scenes
        GameObject.DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        playerInstance = PlayerController.playerControllerInst; //Store reference to instance of player.
    }

    /* Function: checkForDemoEnd
        Checks if 2 quests are completed and 3 enemies are defeated. If true, starts endDemo() coroutine.
    */
    public void checkForDemoEnd()
    {
        Debug.Log("Checking.....");
        if (numberOfQuestsCompleted == 2 && numberOfEnemiesBeaten == 3)
        {
            Debug.Log("Ready to end.....");
            StartCoroutine(endDemo());
        }
    }

    /* Function: endDemo
        Coroutine. Begins a six second timer, and fades in the end demo screen. After six seconds restartGame() is called.
    */
    IEnumerator endDemo()
    {
        yield return new WaitForSeconds(6);
        demoEndBG.SetActive(true);
        demoEndBGAnim.SetBool("demoEnded", true);
        demoEndText.SetActive(true);
        demoEndTextAnim.SetBool("demoEnded", true);
        Debug.Log("Ending.....");
        yield return new WaitForSeconds(6);
        restartGame();
    }

    /* Function: LoadBattle

        Called in PlayerController.OnTriggerEnter() when the player collides with an enemy. Accesses the enemy's stats and assigns them to enemyStats. Sets enemyEncounteredByPlayer to the enemy's ID.
        Deactivates Overworld UI. Sets playerLastOverworldPosition to the player's current position. Loads the Battle Scene.

       Parameters:

          _enemy - The enemy encountered that will be taking part in battle.

    */
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
        SceneManager.LoadScene("BattleTestScene");
    }

    public IDictionary<string, int> SendEnemyStats()
    {
        return enemyStats;
    }

    /* Function: LoadOverwoldAfterBattle

        Called in BattleManager.Wait() when the player has won a battle. If playerWonLastBattle is true, enemyEncounteredByPlayer is added to defeatedEnemiesIDs and numberOfEnemiesBeaten 
        is incremented. Activates Overworld UI. Sets player's current position to playerLastOverworldPosition. Loads the Overworld Scene. checkForDemoEnd() called.

    */
    public void LoadOverwoldAfterBattle()
    {
        playerInstance.gameObject.transform.position = playerLastOverworldPosition - new Vector3(0, 0, 3);
        SceneManager.LoadScene("Overworld");
        overworldUI.gameObject.SetActive(true);
        if (playerWonLastBattle)
        {
            defeatedEnemiesIDs.Add(enemyEncounteredByPlayer);
            numberOfEnemiesBeaten++;
        }
        checkForDemoEnd();
        enemyStats = null;
        //enemyEncounteredByPlayer = null;
    }

    public void setItemAsCollected(int ID)
    {
        collectedInventoryItemIDs.Add(ID);
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

    /* Function: restartGame
        Destroys the player, loads the main menu, then destroys the game manager.
    */
    public void restartGame()
    {
        Destroy(playerInstance.gameObject);
        SceneManager.LoadScene("MainMenu");
        Destroy(this.gameObject);
    }

    //SCENE LOADED MANAGEMENT

    /* Function: OnEnable
        Unity Function. Used to subscribe to the event where levels finish loading.
    */
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    /* Function: OnDisable
        Unity Function. Used to unsubscribe to the event where levels finish loading.
    */
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    /* Function: OnSceneFinishedLoading
        Checks if the scene that has just been loaded is the overowrld. If firstOverworldLoad is true, all enemies and items are found by searching for their tags. Consumable items and Inventory items
        have to be found separately as Unity function FindGameObjectsWithTag() only takes one parameter. The lists to store these two types of items are then merged, converted into an array and stored
        in items. Enemies are all stored in enemies.

        enemies and items are both iterated through. Where the ID of the current element is found in defeatedEnemiesIDs/collectedInventoryItemIDs, the game object is set to inactive.
    */
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            if (firstOverworldLoad)
            {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject[] _items = GameObject.FindGameObjectsWithTag("InventoryItem");
                GameObject[] _consumables = GameObject.FindGameObjectsWithTag("ConsumableItem");
                List<GameObject> temp = new List<GameObject>();
                temp.AddRange(_items);
                temp.AddRange(_consumables);
                items = temp.ToArray();
            }

            foreach (GameObject enemy in enemies)
            {
                Enemy _enemy = (Enemy)enemy.gameObject.GetComponent(typeof(Enemy));
                if (defeatedEnemiesIDs.Contains(_enemy.id))
                {
                    _enemy.gameObject.SetActive(false);
                }
            }

            foreach (GameObject item in items)
            {
                InventoryItem _item = (InventoryItem)item.gameObject.GetComponent(typeof(InventoryItem));
                if (collectedInventoryItemIDs.Contains(_item.id))
                {
                    _item.gameObject.SetActive(false);
                }
            }
        }
    }
}
