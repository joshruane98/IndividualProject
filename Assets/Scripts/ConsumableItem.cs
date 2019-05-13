using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class: ConsumableItem
//The class to represent items stored in the inventory than can be consumed by the player. Extends InventoryItem.
public class ConsumableItem : InventoryItem
{
    /*Function: consumeItem
        Called when the Consume button is clicked in the invenotry. Stores a reference to the player. Checks name of item and dependent on this carries out appropriate action.
        Only HealthPotion implemented for demo. Calls player.GainHealth(20).
    */
    public void consumeItem()
    {
        PlayerController player = (PlayerController)GameObject.Find("Player").GetComponent(typeof(PlayerController));
        if (itemName == "Health Potion")
        {
            player.GainHealth(20);
        } 

        Debug.Log("Item was consumed");
    }
}
