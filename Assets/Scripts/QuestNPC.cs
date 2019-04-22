using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NPC
{
    [SerializeField] string[] questCompleteDialogue;
    [SerializeField] string[] initialDialogue;
    [SerializeField] string itemNeeded;
    public override void interactAction()
    {
        gameManager = GameManager.gameManagerInst;
        gameManager.freezeGameWorld();
        dialogueCanvas.gameObject.SetActive(true);

        if (checkIfPlayerHasItem() == true)
        {
            dialogue = questCompleteDialogue;
        }
        else if (checkIfPlayerHasItem() == false)
        {
            dialogue = initialDialogue;
        }
        dialogueDisplay.text = dialogue[0];
    }

    public bool checkIfPlayerHasItem()
    {
        PlayerController player = player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        if (player.inventory.getItemByName(itemNeeded) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
