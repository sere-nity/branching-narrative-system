using System.Collections;
using System.Collections.Generic;
using Contagion.Inventory;
using UnityEngine;

namespace Contagion.Lua_Interfacing.Functions
{
    public static class InventoryLuaFunctions
    {
        public static void Register()
        {
            LuaFunctionRegistry.Register("GainItem", typeof(InventoryLuaFunctions));
            LuaFunctionRegistry.Register("RemoveItem", typeof(InventoryLuaFunctions));
        }

        public static void GainItem(string itemName)
        {
            SingletonMonoBehaviour<InventoryManager>.Singleton.AddItem(itemName);
            Debug.Log($"Gained item: {itemName}");
        }
        
        public static void RemoveItem(string itemId)
        {
            if (SingletonMonoBehaviour<InventoryManager>.Singleton != null)
            {
                SingletonMonoBehaviour<InventoryManager>.Singleton.RemoveItem(itemId);
            }
            else
            {
                Debug.LogError("InventoryManager not found in scene!");
            }
        }
    }
    
}

