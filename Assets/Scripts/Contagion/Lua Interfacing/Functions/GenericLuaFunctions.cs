using System;
using UnityEngine;

namespace Contagion.Lua_Interfacing.Functions
{
    public static class GenericLuaFunctions
    {
        public static Action<string> OnLuaVariableChanged;

        public static void Register()
        {
            LuaFunctionRegistry.Register("SetVariableValue", typeof(GenericLuaFunctions));
        }

        public static void SetVariableValue(string variableName, object newValue)
        {
            if (string.IsNullOrEmpty(variableName))
            {
                Debug.LogErrorFormat("SetVariableValue(): Trying to set an empty boolean.");
            }
            ArticyUtils.SetTableValue("Variable", variableName, newValue);
            if (variableName.Equals("Actor") || variableName.Equals("Conversant") || OnLuaVariableChanged == null)
            {
                return;
            }
            try
            {
                OnLuaVariableChanged(variableName);
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }
    }
}