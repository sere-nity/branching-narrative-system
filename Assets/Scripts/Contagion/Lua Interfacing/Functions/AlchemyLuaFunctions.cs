using Contagion.Essence_System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion.Lua_Interfacing.Functions
{
    public static class AlchemyLuaFunctions
    {
        public static void Register()
        {
            LuaFunctionRegistry.Register("SetEssenceModifier", typeof(AlchemyLuaFunctions));
        }
        
        // public static void AlchemyFunction()
        // {
        //     try {
        //         
        //     } catch (System.Exception e) {
        //         Debug.LogError($"Failed to perform alchemy function");
        //         Debug.LogError(e);
        //     }
        // }
        
        public static void SetEssenceModifier(string variableName)
        {
            int modifierValue = DialogueLua.GetVariable(variableName).AsInt;
            EssenceManager.Singleton.RecordModifier(variableName, modifierValue);
        }
    }
}