using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class: InventoryItem
//The class to represent items stored in the inventory.
public class InventoryItem : MonoBehaviour
{
    public int id;
    public string itemName;
    public string itemDescription;

    private void OnDestroy()
    {
        Debug.Log("Inventory Item Destroyed");
    }
}
