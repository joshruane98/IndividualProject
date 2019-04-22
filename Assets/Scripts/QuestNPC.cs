using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NPC
{
    [SerializeField] string[] questCompleteDialogue;
    [SerializeField] string[] initialDialogue;
    [SerializeField] string itemNeeded;
    [SerializeField] int rewardGoldAmount;
    PlayerController player;

    public override void interactAction()
    {
        gameManager = GameManager.gameManagerInst;
        gameManager.freezeGameWorld();
        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        dialogueCanvas.gameObject.SetActive(true);

        if (checkIfPlayerHasItem() == true)
        {
            dialogue = questCompleteDialogue;
            player.gainMoney(rewardGoldAmount);
        }
        else if (checkIfPlayerHasItem() == false)
        {
            dialogue = initialDialogue;
        }
        dialogueDisplay.text = dialogue[0];
    }

    public bool checkIfPlayerHasItem()
    {
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
