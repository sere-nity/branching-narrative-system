using UnityEngine;

namespace Contagion.Lua_Interfacing.Functions
{
    public static class QuestLuaFunctions
    {
        public static void Register()
        {
            LuaFunctionRegistry.Register("StartQuest", typeof(QuestLuaFunctions));
        }
        
        public static void StartQuest(string questName)
        {
            try {
                
            } catch (System.Exception e) {
                Debug.LogError($"Failed to start quest: {questName}");
                Debug.LogError(e);
            }
        }
    }
}