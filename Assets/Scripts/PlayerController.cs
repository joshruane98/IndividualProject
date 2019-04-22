using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : BattleCharacter {
    private int XP;
    private int level;
    public int moneyBalance;
    //public ArrayList inventory;
    public Interactable interactableObj; //Nearby object that can currently be interacted with.
    public bool movementDisabled; //Identifies if player is currently in Battle. Used to disable movement controls when in battle.

    public static PlayerController onlyPlayerController; //Used for checking to ensure one instance of PlayerController persists across all scenes.
    public Inventory inventory;
    GameManager gameManager;

    //UI
    public GameObject interactPrompt;
    public Text moneyDisplay;

    // Used Awake instead of Start to ensure that all player stats are initialised before being read in battle.
    void Awake () {
        if (onlyPlayerController != null)
        {
            Debug.Log("THERES ALREADY A PLAYER");
            Destroy(this.gameObject);
        }
        else if (onlyPlayerController == null)
        {
            Debug.Log("I'M THE ONLY PLAYER");
        }
        onlyPlayerController = this;
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
        reflex = 100;
        XP = 0;
        level = 1;
        moneyBalance = 0;

        anim = GetComponent<Animator>();
    }

    void Start()
    {
        gameManager = GameManager.gameManagerInst; //Called in start as GameManager is instantiated at awake so this ensures a reference can be created.
    }

    // Update is called once per frame
    void Update () {
        movePlayerOnInput();
        checkForNearbyInteractables();
    }

    //MOVEMENT FUNCTIONS
    void movePlayerOnInput()
    {
        if (!movementDisabled)
        {
            if (Input.GetKey("up"))
            {
                goInDirection("forward");
                anim.SetBool("IsWalking", true);
            }
            else if (Input.GetKey("down"))
            {
                goInDirection("backward");
                anim.SetBool("IsWalking", true);
            }
            else if (Input.GetKey("left"))
            {
                goInDirection("left");
                anim.SetBool("IsWalking", true);
            }
            else if (Input.GetKey("right"))
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

    public void setBattlePosition()
    {
        transform.position = new Vector3(-0.52f, -1.90f, -0.08f);
        transform.Translate(new Vector3(0, 0, 0) * Time.deltaTime);
        anim.SetBool("IsWalking", false);
    }

    void goInDirection(string direction)
    {
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

    //CHECK COLLISIONS
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            collision.gameObject.SetActive(false);
            moneyBalance++;
            moneyDisplay.text = moneyBalance.ToString();
        }
        
        if (collision.gameObject.CompareTag("InventoryItem"))
        {
            collision.gameObject.SetActive(false);
            inventory.addItem((InventoryItem)collision.gameObject.GetComponent(typeof(InventoryItem)));
        }
        
        if (collision.gameObject.CompareTag("ConsumableItem"))
        {
            collision.gameObject.SetActive(false);
            inventory.addItem((ConsumableItem)collision.gameObject.GetComponent(typeof(ConsumableItem)));
        }
        
    }

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
    private void checkForNearbyInteractables()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1 , transform.TransformDirection(Vector3.forward), out hit, 2f, layerMask))
        {
            Debug.DrawRay(transform.position + Vector3.up * 1 , transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Press E to interact.");
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
