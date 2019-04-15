﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    IDictionary<int, InventoryItem> inventory;
    int maxInventorySize;

    //UI
    [SerializeField] GameObject inventoryDisplay;
    [SerializeField] Button slot1Button;
    [SerializeField] Button slot2Button;
    [SerializeField] Button slot3Button;
    [SerializeField] Button slot4Button;
    [SerializeField] Button slot5Button;
    [SerializeField] Text descriptionTextBox;

    void Awake()
    {
        inventory = new Dictionary<int, InventoryItem>()
        {
            {1, null },
            {2, null },
            {3, null },
            {4, null },
            {5, null }
        };

        maxInventorySize = 5;

    }

    public InventoryItem getItemInSlot(int slotNum)
    {
        return inventory[slotNum];
    }

    public void addItem(InventoryItem itemToAdd)
    {
        Debug.Log(itemToAdd.itemName);
        for (int i = 1; i <= maxInventorySize; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = itemToAdd;
                Debug.Log(inventory[i].itemName);
                break;
            }
        }
        Debug.Log(inventory[1].itemName);
    }

    public void displayInventory()
    {
        inventoryDisplay.SetActive(true);

        //Nicer ways to do this but it works for now
        if (inventory[1] != null)
        {
            slot1Button.gameObject.SetActive(true);
            Debug.Log(inventory[1].itemName);
            slot1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = inventory[1].itemName;
        }
        if (inventory[2] != null)
        {
            slot2Button.gameObject.SetActive(true);
            slot2Button.gameObject.GetComponent<Text>().text = inventory[2].itemName;
        }
        if (inventory[3] != null)
        {
            slot3Button.gameObject.SetActive(true);
            slot3Button.gameObject.GetComponent<Text>().text = inventory[3].itemName;
        }
        if (inventory[4] != null)
        {
            slot4Button.gameObject.SetActive(true);
            slot4Button.gameObject.GetComponent<Text>().text = inventory[4].itemName;
        }
        if (inventory[5] != null)
        {
            slot5Button.gameObject.SetActive(true);
            slot5Button.gameObject.GetComponent<Text>().text = inventory[5].itemName;
        }
    }

    public void closeInventory()
    {
        slot1Button.gameObject.SetActive(false);
        slot2Button.gameObject.SetActive(false);
        slot3Button.gameObject.SetActive(false);
        slot4Button.gameObject.SetActive(false);
        slot5Button.gameObject.SetActive(false);
        inventoryDisplay.SetActive(false);
    }
}
