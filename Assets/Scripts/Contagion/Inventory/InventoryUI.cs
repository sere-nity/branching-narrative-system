using System;
using UnityEngine.UI;
using UnityEngine;

namespace Contagion.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] 
        private Transform itemsContainer;
        [SerializeField] 
        private InventorySlot inventorySlotTemplate;
        
        private void OnEnable()
        {
            SingletonMonoBehaviour<InventoryManager>.Singleton.OnInventoryChanged += RefreshInventory;
            RefreshInventory();
        }

            private void OnDisable()
            {
                if (SingletonMonoBehaviour<InventoryManager>.Singleton != null)
                {
                    SingletonMonoBehaviour<InventoryManager>.Singleton.OnInventoryChanged -= RefreshInventory;
                }
            }

            public void RefreshInventory()
            {
                Debug.Log("REFRESHING INVENTORY");
                // Clear existing items
                foreach (Transform child in itemsContainer)
                {
                    if (child.transform != inventorySlotTemplate.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
        
            // Add current items
            var items = SingletonMonoBehaviour<InventoryManager>.Singleton.GetPlayerItems();
            foreach (var item in items)
            {
                Debug.Log("Adding item to inventory: " + item.displayName);
                var slot = Instantiate(inventorySlotTemplate, itemsContainer);
                slot.SetItemSlot(item.displayName, item.icon);
                slot.gameObject.SetActive(true);
            }
        }
    }
}