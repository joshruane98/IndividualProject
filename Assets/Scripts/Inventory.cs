using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class: Inventory
//The class implementing the player's inventory.
public class Inventory : MonoBehaviour
{
    //Variable: inventory
    //A dictionary containing key-value pairs of slot number and the inventoryItem in that slot.
    IDictionary<int, InventoryItem> inventory;
    int maxInventorySize;
    //Variable: selectedSlot
    //The number of the currently selected slot, used as the value to access an item in the inventory dictionary.
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

    //Function: Awake
    //Unity function with unique behaviour. Set-up function used for initialisation.
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

    /* Function: getItemInSlot

       Parameters:

          slotNum - The slot number needed to be accessed.

       Returns:

          The InventoryItem in the requested slot.

    */
    public InventoryItem getItemInSlot(int slotNum)
    {
        return inventory[slotNum];
    }

    /* Function: getItemByName

       Parameters:

          itemRequired - The name of the requested item.

       Returns:

          The requested InventoryItem. If the item is not found, null is returned.

    */
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

    /* Function: addItem
       
       Adds a newly collected item to the inventory. Currently contains no checking for if inventory is full but this isn't necessary at present as
       the demo contains less than the maximum number of items.

       Parameters:
            
          itemToAdd - The item to be added to the inventory.

    */
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

    /* Function: removeItemByName

       Parameters:

          itemToBeRemoved - The name of the item to be removed.

    */
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

    /* Function: removeItemInSlot

       Parameters:

          slot - The slot number needed to be accessed.

    */
    public void removeItemInSlot(int slot)
    {
        inventory[slot] = null;
        consumeButton.gameObject.SetActive(false);
        selectedSlot = 0;
        organiseInventory();
        displayInventory();
    }

    /* Function: setSelectedSlot

       Called when a slot is clicked on in the menu. Displays the description of the item in the selected slot, as well as the 'Consume'
       button if the item in the slot is of type ConsumableItem.

       Parameters:

          slotNumber - The slot number selected.

    */
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

    /*Function: consumeButtonAction
        Called when the consume button is pressed. Double checks if the item in the selected slot is of type ConsumableItem and, if so, calls
        the items consumeItem() function. The double check is neccesary due to the utilisation of polymorphism here and the InventoryItem class
        not containing a consumeItem() function.
    */
    public void consumeButtonAction()
    {
        if (inventory[selectedSlot].GetType() == typeof(ConsumableItem))
        {
            ConsumableItem consumable = (ConsumableItem)inventory[selectedSlot];
            consumable.consumeItem();
        }
        removeItemInSlot(selectedSlot);
    }

    //Function: organiseInventory
    //Closes any gaps in the inventory when an item is removed.
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

    //Function: displayInventory
    //Displays the inventory UI on screen when the R key is pressed.
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

    //Function: closeInventory
    //Closes the inventory UI on screen when the cross is clicked.
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
