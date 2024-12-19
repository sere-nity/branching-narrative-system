using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contagion.Inventory
{
    public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
    {
        private Dictionary<string, ItemData> itemLookup;
        private List<ItemData> playerItems = new List<ItemData>();
        public event Action OnInventoryChanged;


        public InventoryUI inventoryUI;

        protected override void Awake()
        {
            base.Awake();
            LoadAllItems();
        }
        private void LoadAllItems()
        {
            itemLookup = new Dictionary<string, ItemData>();
            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            foreach (var item in allItems)
            {
                itemLookup[item.id] = item;
            }
        }
        
        public bool AddItem(string itemId)
        {
            if (itemLookup.TryGetValue(itemId, out ItemData item))
            {
                playerItems.Add(item);
                OnInventoryChanged?.Invoke(); // Notify listeners
                Debug.Log($"Added {item.displayName} to inventory");
                return true;
            }
            Debug.LogWarning($"Item with ID {itemId} not found in database");
            return false;
        }
    
        public bool RemoveItem(string itemId)
        {
            var item = playerItems.Find(i => i.id == itemId);
            if (item != null)
            {
                playerItems.Remove(item);
                OnInventoryChanged?.Invoke(); // Notify listeners
                Debug.Log($"Removed {item.displayName} from inventory");
                return true;
            }
            Debug.LogWarning($"Item with ID {itemId} not found in inventory");
            return false;
        }
    
        public List<ItemData> GetPlayerItems()
        {
            return new List<ItemData>(playerItems);
        }

        public void ToggleInventory()
        {
            if (inventoryUI.gameObject != null)
            {
                inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
            }
        }


    }
}