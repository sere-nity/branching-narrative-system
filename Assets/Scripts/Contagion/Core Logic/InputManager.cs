using Contagion.Diary_System;
using Contagion.Inventory;
using UnityEngine;

namespace Contagion.Core_Logic
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ToggleDiary();
            }

            // Add more input handling as needed
        }

        private void ToggleInventory()
        {
            SingletonMonoBehaviour<InventoryManager>.Singleton.ToggleInventory();
        }

        private void ToggleDiary()
        {
            SingletonMonoBehaviour<DiaryController>.Singleton.ToggleDiary();
        }
    }
}