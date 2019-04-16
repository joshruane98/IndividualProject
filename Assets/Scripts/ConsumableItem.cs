using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : InventoryItem
{
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
