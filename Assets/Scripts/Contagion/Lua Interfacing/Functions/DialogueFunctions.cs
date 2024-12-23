using UnityEngine;

namespace Contagion.Lua_Interfacing.Functions
{
    public class DialogueFunctions : MonoBehaviour
    {
        private void OnEnable()
        {
            InventoryLuaFunctions.Register();
            QuestLuaFunctions.Register();
            AlchemyLuaFunctions.Register();
        }
    }
    
}

