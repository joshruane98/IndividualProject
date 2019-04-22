﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public string itemName;
    public string itemDescription;

    private void OnDestroy()
    {
        Debug.Log("Inventory Item Destroyed");
    }
}
