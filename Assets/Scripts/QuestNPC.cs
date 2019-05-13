using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class: QuestNPC
//Class for Non-player characters that deliver fetch-quests to the player. Extends the NPC Class.
public class QuestNPC : NPC
{
    //Variable: questCompleteDialogue
    //An array of strings that represent each phrase of the NPC's dialogue. These sentences are displayed when the quest is complete. Length and sentences can be defined in the Unity Editor.
    [SerializeField] string[] questCompleteDialogue;
    //Variable: initialDialogue
    //An array of strings that represent each phrase of the NPC's dialogue. These sentences are displayed before the quest is complete.Length and sentences can be defined in the Unity Editor.
    [SerializeField] string[] initialDialogue;
    //Variable: itemNeeded
    //The name of the item the NPC asks for.
    [SerializeField] string itemNeeded;
    //Variable: rewardGoldAmount
    //The amount of gold to be given as a reward upon return to the NPC with itemNeeded.
    [SerializeField] int rewardGoldAmount;
    //Variable: player
    //Reference to the player.
    PlayerController player;

    public AudioClip happySound;

    //Variable: rewardGiven
    //Tracks if the NPC has already given the reward to the player to prevent it being given multiple times. Initialised as false.
    bool rewardGiven = false;

    /*Function: interactAction
        Overrides virtual method in Interactable class and functionality in NPC. Calls gameManager.freezeGameWorld() to stop layer movement. Finds reference to the player.
        Calls checkIFPlayerHasItem(). If this returns true, sets dialogue to questCompletedDialogue, checks if rewardGiven. If rewardGiven is still false, calls player.gainMoney(rewardGoldAmount),
        increments gameManager.numberOfQuestsCompleted and calls gameManager.checkForDemoEnd(). If checkIfPlayerHasItem() returns false, dialogue is set to initialDialogue.
        Then activates the dialogue box UI, plays the talk sound effect, and diaplays the first sentence.
    */
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
                gameManager.numberOfQuestsCompleted++;
                gameManager.checkForDemoEnd();
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

    /*Function: interactAction
        Checks if player.inventory.getItemByName(itemNeeded) returns null. If it does, returns false. If it doesn't, calls player.inventory.removeItemByName(itemNeeded), 
        then returns true.
    */
    public bool checkIfPlayerHasItem()
    {
        if (player.inventory.getItemByName(itemNeeded) != null)
        {
            player.inventory.removeItemByName(itemNeeded);
            return true;
        }
        else
        {
            return false;
        }
    }
}
