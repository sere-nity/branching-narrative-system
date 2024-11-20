using System; 
using PixelCrushers.DialogueSystem;

namespace Contagion.Lua_Interfacing.Functions
{
    public static class LuaFunctionRegistry
    {
        public static void Register(string name, Type type)
        {
            try
            {
                Lua.RegisterFunction(name, null, type.GetMethod(name));
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Error registering Lua function {name}: {ex}");
            }
        }
    }
}

