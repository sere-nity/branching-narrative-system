using System.Collections.Generic;
using Contagion.Lua_Interfacing;
using PixelCrushers.DialogueSystem;

namespace Contagion.Metric
{
    /// <summary>
    /// Class for storing the data concerning the result of a check. Important data includes
    /// 
    /// </summary>
    public class CheckResult
    {
        public CheckType checkType;

        public int baseDifficulty;
        
        
        // TODO - encapsulate better
        public List<string> requriedIngredients; 
        public List<string> missingIngredients;
        public List<CheckModifier> applicableModifiers;
        
        public ResultType resultType;

        public CheckResult(CheckType checkType)
        {
            this.checkType = checkType;
        }
        
        /* Do we even want this variable - should the crafting
           only be unlocked if they have the required ingredients 
           or can they attempt it regardless? */
        // public bool isLocked; 

        public void PopulateIngredientLists(DialogueEntry entry)
        {
            requriedIngredients = ArticyFieldMapper.GetRequiredIngredients(entry);
            missingIngredients = ArticyFieldMapper.GetMissingIngredients(entry);
        }

        public void PopulateApplicableModifiers(DialogueEntry entry)
        {
            applicableModifiers = ArticyFieldMapper.GetApplicableModifiers(entry);
        }
        
    }
}