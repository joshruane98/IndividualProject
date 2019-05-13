using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class: NPC
//Class for Non-player characters that can be spoken to in the world. Extends the Interactable Class.
public class NPC : Interactable
{
    //Variable: gameManager
    //Reference to the Game Manager.
    protected GameManager gameManager;
    [SerializeField] string npcName;
    //Variable: dialogue
    //An array of strings that represent each phrase of the NPC's dialogue. Length and sentences can be defined in the Unity Editor.
    [SerializeField] protected string[] dialogue;
    //Variable: i
    //Counter used when iterating through dialogue array.
    int i;
    public Canvas dialogueCanvas;
    public Text dialogueDisplay;
    //public GameObject dialogueBackground;
    //public Button nextSentenceButton;
    public AudioSource source;
    public AudioClip talkSound;

    //Function: Start
    //Unity function with unique behaviour. Set-up function. Sets the reference to the game manager. This is called in start as GameManager is instantiated at awake so this ensures a reference can be created.
    private void Start()
    {
        gameManager = GameManager.gameManagerInst;
        if (gameManager != null)
        {
            Debug.Log("NPC has GM");
        }
    }

    //Function: interactAction
    //Overrides virtual method in Interactable class. Calls gameManager.freezeGameWorld() to stop layer movement. Then activates the dialogue box UI, plays the talk sound effect, and diaplays the first sentence.
    public override void interactAction()
    {
        gameManager = GameManager.gameManagerInst;
        gameManager.freezeGameWorld();
        dialogueCanvas.gameObject.SetActive(true);
        //dialogueDisplay.gameObject.SetActive(true);
        //dialogueBackground.SetActive(true);
        //nextSentenceButton.gameObject.SetActive(true);
        source.PlayOneShot(talkSound);
        dialogueDisplay.text = dialogue[0];
    }

    //Function: displayNextSentence
    //Called when Continue button is clicked. Iterates through to retrieve next element of dialogue array. If button is pressed while last sentence is displayed, the dialogue box is hidden and gameManager.unfreezeGameWorld() called.
    public void displayNextSentence()
    {
        i++;
        if (i <= dialogue.Length - 1)
        {
            dialogueDisplay.text = dialogue[i];
        }
        else
        {
            //No more dialogue so continue game
            dialogueCanvas.gameObject.SetActive(false);
            i = 0;
            //dialogueDisplay.gameObject.SetActive(false);
            //dialogueBackground.SetActive(false);
            //nextSentenceButton.gameObject.SetActive(false);
            gameManager.unfreezeGameWorld();

        }
    }
}
