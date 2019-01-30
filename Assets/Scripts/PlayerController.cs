using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private int maxHealth;
    private int health;
    private int baseAttackPower; //basic attack power
    private int attackPower; //current attack power including any effects
    private int baseDefence; //basic defence stat
    private int defence; //current defence including any effects
    private int bravery;
    private int reflex;
    private int XP;
    private int level;
    private int moneyBalance;
    public Interactable interactableObj; //Nearby object that can currently be interacted with.

	// Used Awake instead of Start to ensure that all player stats are initialised before being read in battle.
	void Awake () {
        maxHealth = 50;
        health = maxHealth;
        baseAttackPower = 10;
        attackPower = baseAttackPower;
        baseDefence = 50;
        defence = baseDefence;
        bravery = 15;
        reflex = 30;
        XP = 0;
        level = 1;
        moneyBalance = 0;
    }
	
	// Update is called once per frame
	void Update () {
        movePlayerOnInput();
        checkForNearbyInteractables();
    }

    //MOVEMENT FUNCTIONS
    void movePlayerOnInput()
    {
        if (Input.GetKey("up"))
        {
            goForward();
        }
        else if (Input.GetKey("down"))
        {
            goBack();
        }
        else if (Input.GetKey("left"))
        {
            goLeft();
        }
        else if (Input.GetKey("right"))
        {
            goRight();
        }
    }

    void goForward()
    {
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime);
    }

    void goBack()
    {
        transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime);
    }

    void goLeft()
    {
        transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime);
    }

    void goRight()
    {
        transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
    }

    //HEALTH FUNCTIONS
    int getCurrentHealth()
    {
        return health;
    }

    int getMaxHealth()
    {
        return maxHealth;
    }

    void setMaxHealth(int amount)
    {
        maxHealth = amount;
    }

    void gainHealth(int amount)
    {
        health += amount;
    }

    void loseHealth(int amount)
    {
        health -= amount;
    }

    //ATTACK POWER FUNCTIONS
    int getCurrentAttackPower()
    {
        return attackPower;
    }

    int getBaseAttackPower()
    {
        return baseAttackPower;
    }

    void setBaseAttackPower(int amount)
    {
        baseAttackPower = amount;
    }

    void increaseCurrentAttackPower(int amount)
    {
        attackPower += amount;
    }

    void resetCurrentAttackPower()
    {
        attackPower = getBaseAttackPower();
    }

    //DEFENCE STAT FUNCTIONS
    int getCurrentDefence()
    {
        return defence;
    }

    int getBaseDefence()
    {
        return baseDefence;
    }

    void setBaseDefence(int amount)
    {
        baseDefence = amount;
    }

    void increaseCurrentDefence(int amount)
    {
        attackPower += amount;
    }

    void resetCurrentDefence()
    {
        defence = getBaseDefence();
    }

    //GET BRAVERY AND REFLEX
    int getBravery()
    {
        return bravery;
    }

    public int getReflex()
    {
        return reflex;
    }
    //CHECK COLLISIONS
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with enemy");
        }
    }

    //LOOK FOR INTERACTABLE
    private void checkForNearbyInteractables()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Press E to interact.");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interacted");
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
