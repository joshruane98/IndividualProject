using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    IDictionary<int, InventoryItem> inventory;
    int maxInventorySize;
    int selectedSlot;

    //UI
    [SerializeField] GameObject inventoryDisplay;
    [SerializeField] Button slot1Button;
    [SerializeField] Button slot2Button;
    [SerializeField] Button slot3Button;
    [SerializeField] Button slot4Button;
    [SerializeField] Button slot5Button;
    [SerializeField] Text descriptionTextBox;
    [SerializeField] Button consumeButton;

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

    public InventoryItem getItemByName(string itemRequired)
    {
        for (int i = 1; i <= maxInventorySize; i++)
        {

            if (inventory[i] != null && inventory[i].itemName == itemRequired)
            {
                return inventory[i];
            }
        }
        return null;
    }

    public void addItem(InventoryItem itemToAdd)
    {
        InventoryItem _itemToAdd = itemToAdd;
        itemToAdd.gameObject.transform.parent = null;
        GameObject.DontDestroyOnLoad(itemToAdd.gameObject);
        Debug.Log(itemToAdd.itemName);
        for (int i = 1; i <= maxInventorySize; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = _itemToAdd;
                Debug.Log(inventory[i].itemName);
                break;
            }
        }
        Debug.Log(inventory[1].itemName);
    }

    public void removeItemByName(string itemToBeRemoved)
    {
        //Primarily for removing quest items, not consumables such as potions.
        for (int i = 1; i <= maxInventorySize; i++)
        {
            if (inventory[i].itemName == itemToBeRemoved)
            {
                inventory[i] = null;
                break;
            }
        }
        organiseInventory();
    }

    public void removeItemInSlot(int slot)
    {
        inventory[slot] = null;
        consumeButton.gameObject.SetActive(false);
        selectedSlot = 0;
        organiseInventory();
        displayInventory();
    }

    public void setSelectedSlot(int slotNumber)
    {
        //When a slot button is clicked...
        selectedSlot = slotNumber;
        descriptionTextBox.text = inventory[selectedSlot].itemDescription;
        if (inventory[selectedSlot].GetType() == typeof(ConsumableItem))
        {
            consumeButton.gameObject.SetActive(true);
        }
        else
        {
            consumeButton.gameObject.SetActive(false);
        }
    }

    public void consumeButtonAction()
    {
        if (inventory[selectedSlot].GetType() == typeof(ConsumableItem))
        {
            ConsumableItem consumable = (ConsumableItem)inventory[selectedSlot];
            consumable.consumeItem();
        }
        removeItemInSlot(selectedSlot);
    }

    public void organiseInventory()
    {
        //Used to close gaps in inventory when items removed
        int i = 1;
        while (i < maxInventorySize)
        {
            if (inventory[i] == null && inventory[i+1] != null)
            {
                inventory[i] = inventory[i + 1];
                inventory[i + 1] = null;
            }
            i++;
        }
    }

    public void displayInventory()
    {
        inventoryDisplay.SetActive(true);

        //Nicer ways to do this but it works for now
        if (inventory[1] != null)
        {
            slot1Button.gameObject.SetActive(true);
            slot1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = inventory[1].itemName;
        }
        else
        {
            slot1Button.gameObject.SetActive(false);
        }
        if (inventory[2] != null)
        {
            slot2Button.gameObject.SetActive(true);
            slot2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = inventory[2].itemName;
        }
        else
        {
            slot2Button.gameObject.SetActive(false);
        }
        if (inventory[3] != null)
        {
            slot3Button.gameObject.SetActive(true);
            slot3Button.transform.Find("Text").gameObject.GetComponent<Text>().text = inventory[3].itemName;
        }
        else
        {
            slot3Button.gameObject.SetActive(false);
        }
        if (inventory[4] != null)
        {
            slot4Button.gameObject.SetActive(true);
            slot4Button.transform.Find("Text").gameObject.GetComponent<Text>().text = inventory[4].itemName;
        }
        else
        {
            slot4Button.gameObject.SetActive(false);
        }
        if (inventory[5] != null)
        {
            slot5Button.gameObject.SetActive(true);
            slot5Button.transform.Find("Text").gameObject.GetComponent<Text>().text = inventory[5].itemName;
        }
        else
        {
            slot5Button.gameObject.SetActive(false);
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

    public void OnDestroy()
    {
        Debug.Log("Inventory destroyed");
    }
}
