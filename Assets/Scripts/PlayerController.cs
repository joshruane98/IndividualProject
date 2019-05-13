using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class: PlayerController
//The class responsible for all player behaviour. Extends the Battle Character Class.
public class PlayerController : BattleCharacter {
    private int XP;
    private int level;
    //Variable: moneyBalance
    //The total ammount of gold the player currently has.
    public int moneyBalance;
    //Variable: interactableObj
    //Stores a reference to the interactable object currently in range of the player, allowing them to interact with it. This is destroyed when an object is no longer in range.
    public Interactable interactableObj;
    //Variable: movementDisabled
    //If true, prevents the player from moving. Utilised in battles and when talking to NPC's.
    public bool movementDisabled;

    //Variable: playerControllerInst
    //Stores the instance of the player controller. Used for checking to ensure one instance of PlayerController persists across all scenes
    public static PlayerController playerControllerInst;
    //Variable: inventory
    //Stored reference to the player's inventory.
    public Inventory inventory;
    //Variable: gameManager
    //Reference to the Game Manager.
    GameManager gameManager;

    //UI
    public GameObject interactPrompt;
    public Text moneyDisplay;

    //Audio
    public AudioClip walkSound;
    public AudioClip goldCollectSound;


    /* Function: Awake
        Unity function with unique behaviour. Set-up funtion. 
        Checks to see if an instance of a player is already active. If yes, the new instance is destroyed. If no, playerControllerInst is set to this. All stats are initialised here.
        Used Awake instead of Start to ensure that all player stats are initialised before being read in battle.
    */
    void Awake () {
        if (playerControllerInst != null)
        {
            Debug.Log("THERES ALREADY A PLAYER");
            Destroy(this.gameObject);
        }
        else if (playerControllerInst == null)
        {
            Debug.Log("I'M THE ONLY PLAYER");
        }
        playerControllerInst = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        movementDisabled = false;
        isStunned = false;
        maxHealth = 50;
        health = maxHealth;
        baseAttackPower = 10;
        attackPower = baseAttackPower;
        baseDefence = 50;
        defence = baseDefence;
        bravery = 15;
        reflex = 12;
        XP = 0;
        level = 1;
        moneyBalance = 0;

        anim = GetComponent<Animator>();
    }
    //Function: Start
    //Unity function with unique behaviour. Set-up function. Sets the reference to the game manager. This is called in start as GameManager is instantiated at awake so this ensures a reference can be created.
    void Start()
    {
        gameManager = GameManager.gameManagerInst;
        if (gameManager != null)
        {
            Debug.Log("Player HAS Game Manager");
        }
    }

    //Function: Update
    //Unity function with unique behaviour. Update is called once per frame. Executes the movePlayerOnInput and checkForNearbyInteractables functions.
    void Update () {
        movePlayerOnInput();
        checkForNearbyInteractables();
    }

    //MOVEMENT FUNCTIONS

    /*Function: movePlayerOnInput
        If movement is not disabled, checks for key-presses and calls goInDirection() along with the direction of movement as a parameter. Plays the walking animation when moving. 
        Also handles the key press for opening the inventory.
    */
    void movePlayerOnInput()
    {
        if (!movementDisabled)
        {
            if (Input.GetKey("w"))
            {
                goInDirection("forward");
                anim.SetBool("IsWalking", true);
            }
            else if (Input.GetKey("s"))
            {
                goInDirection("backward");
                anim.SetBool("IsWalking", true);
            }
            else if (Input.GetKey("a"))
            {
                goInDirection("left");
                anim.SetBool("IsWalking", true);
            }
            else if (Input.GetKey("d"))
            {
                goInDirection("right");
                anim.SetBool("IsWalking", true);
            }
            else
            {
                anim.SetBool("IsWalking", false);
            }
        }

        if (Input.GetKey("r"))
        {
            inventory.displayInventory();
        }
    }

    /* Function: goInDirection

       Moves the player in the desired direction utilising transform.Translate. 
       Also rotates the player to face the correct direction using transform.localEulerAngles.

       Parameters:

          direction - The desired movement direction.

    */
    void goInDirection(string direction)
    {
        //source.clip = walkSound;
        if (!source.isPlaying)
        {
            source.PlayOneShot(walkSound);
        }
        if (direction == "forward")
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == "backward")
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == "left")
        {
            transform.localEulerAngles = new Vector3(0, 270, 0);
        }
        else if (direction == "right")
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        transform.Translate(new Vector3(0, 0, 3) * Time.deltaTime);
    }

    public void setBattlePosition()
    {
        transform.position = new Vector3(-0.52f, -1.90f, -0.08f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.Translate(new Vector3(0, 0, 0) * Time.deltaTime);
        anim.SetBool("IsWalking", false);
    }

    //HEALTH FUNCTIONS
    void setMaxHealth(int amount)
    {
        maxHealth = amount;
    }

    //ATTACK POWER FUNCTIONS
    void setBaseAttackPower(int amount)
    {
        baseAttackPower = amount;
    }

    //DEFENCE STAT FUNCTIONS
    void setBaseDefence(int amount)
    {
        baseDefence = amount;
    }

    public void gainMoney(int amount)
    {
        moneyBalance += amount;
        moneyDisplay.text = moneyBalance.ToString();
        source.PlayOneShot(goldCollectSound);
    }

    //CHECK COLLISIONS

    /* Function: OnCollisionEnter
            
    Unity function with unique behaviour. Determines behaviour for when player collides with different objects. 
    Handles the collecting money and collecting of items (and storing them in the inventory).

    Parameters:

      collision - The collision that has occured.

    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            collision.gameObject.SetActive(false);
            gainMoney(1);
        }
        
        if (collision.gameObject.CompareTag("InventoryItem"))
        {
            InventoryItem item = (InventoryItem)collision.gameObject.GetComponent(typeof(InventoryItem));
            collision.gameObject.SetActive(false);
            inventory.addItem(item);
            gameManager.setItemAsCollected(item.id);
        }
        
        if (collision.gameObject.CompareTag("ConsumableItem"))
        {
            ConsumableItem item = (ConsumableItem)collision.gameObject.GetComponent(typeof(ConsumableItem));
            collision.gameObject.SetActive(false);
            inventory.addItem(item);
            gameManager.setItemAsCollected(item.id);
        }
        
    }

    /* Function: OnTriggerEnter

    Unity function with unique behaviour. Determines behaviour for when player intersects with a trigger collider. 
    Used to load battles when an enemy is near a player. Calls gameManager.LoadBattle() with the enemy as a parameter.

    Parameters:

      trigger - The trigger collider intersected with.

    */
    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with enemy");
            Enemy _enemy = (Enemy)trigger.gameObject.GetComponent(typeof(Enemy));
            //Store enemy stats and load battle - these stats will be assigned to the enemy in battle.

            gameManager.LoadBattle(_enemy);
        }
    }

    //LOOK FOR INTERACTABLE

    /* Function: checkForNearbyInteractables
    Utilises a Raycast. If this ray collides with an interactable object then a reference to that object is stored in interactableObj.
    If the E key is pressed while the ray is intersecting with an interactable object, the object's interactAction() function is called. 
    */
    private void checkForNearbyInteractables()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1 , transform.TransformDirection(Vector3.forward), out hit, 2f, layerMask))
        {
            Debug.DrawRay(transform.position + Vector3.up * 1 , transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Press E to interact.");
            interactPrompt.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                //interactableObj.interactAction();
                //Debug.Log("Interacted");
                interactableObj = (Interactable)hit.collider.gameObject.GetComponent(typeof(Interactable));
                interactableObj.interactAction();
            }
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 1, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            if (!movementDisabled)
            {
                interactPrompt.SetActive(false);
            }
        }
    }
}
