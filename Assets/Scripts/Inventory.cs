using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    Item[] inventory = new Item[3];

    public int PickupItem(Item item) {
        for (int i = 0; i < inventory.Length; i++) {
            if(inventory[i] == null) {
                inventory[i] = item;
                return i;
            }
        }

        return -1;
    }

    public Item DropItem() {
        if(inventory[0] == null) { return null; }

        Item droppedItem = inventory[0];
        
        for (int i = 1; i < inventory.Length; i++) {
            inventory[i-1] = inventory[i];
        }

        inventory[inventory.Length - 1] = null;

        return droppedItem;
    }
}
