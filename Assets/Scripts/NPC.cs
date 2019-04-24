using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : Interactable
{
    protected GameManager gameManager;
    [SerializeField] string npcName;
    [SerializeField] protected string[] dialogue;
    int i;
    public Canvas dialogueCanvas;
    public Text dialogueDisplay;
    //public GameObject dialogueBackground;
    //public Button nextSentenceButton;
    public AudioSource source;
    public AudioClip talkSound;
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
