using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contagion.Lua_Interfacing.Functions
{
    public static class InventoryLuaFunctions
    {
        public static void Register()
        {
            LuaFunctionRegistry.Register("GainItem", typeof(InventoryLuaFunctions));
        }

        public static void GainItem(string itemName)
        {
            Debug.Log($"Gained item: {itemName}");
            // Implement actual item gaining logic here
        }
    }
    
}

