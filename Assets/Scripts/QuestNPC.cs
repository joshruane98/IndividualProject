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

    public AudioClip happySound;

    bool rewardGiven = false;

    public override void interactAction()
    {
        gameManager = GameManager.gameManagerInst;
        gameManager.freezeGameWorld();
        player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        dialogueCanvas.gameObject.SetActive(true);

        if (checkIfPlayerHasItem() == true)
        {
            source.PlayOneShot(happySound);
            dialogue = questCompleteDialogue;
            if (rewardGiven == false)
            {
                player.gainMoney(rewardGoldAmount);
                rewardGiven = true;
            }
        }
        else if (checkIfPlayerHasItem() == false)
        {
            source.PlayOneShot(talkSound);
            dialogue = initialDialogue;
        }
        dialogueDisplay.text = dialogue[0];
    }

    public bool checkIfPlayerHasItem()
    {
        if (player.inventory.getItemByName(itemNeeded) != null)
        {
            gameManager.numberOfQuestsCompleted++;
            gameManager.checkForDemoEnd();
            return true;
        }
        else
        {
            return false;
        }
    }
}
