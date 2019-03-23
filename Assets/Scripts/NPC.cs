using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : Interactable
{
    [SerializeField] string npcName;
    [SerializeField] string[] dialogue;
    int i;
    public Text dialogueDisplay;
    public GameObject dialogueBackground;
    public Button nextSentenceButton;
    public override void interactAction()
    {
        dialogueDisplay.gameObject.SetActive(true);
        dialogueBackground.SetActive(true);
        nextSentenceButton.gameObject.SetActive(true);
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
            dialogueDisplay.gameObject.SetActive(false);
            dialogueBackground.SetActive(false);
            nextSentenceButton.gameObject.SetActive(false);
        }
    }
}
