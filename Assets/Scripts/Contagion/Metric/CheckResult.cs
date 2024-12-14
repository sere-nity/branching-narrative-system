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

        public string craftedItemName;

        public CheckResult(CheckType checkType)
        {
            this.checkType = checkType;
        }
        
        /* Do we even want this variable - should the crafting
           only be unlocked if they have the required ingredients 
           or can they attempt it regardless? */
        // public bool isLocked; 

        public void PopulateCraftedItemInfo(DialogueEntry entry)
        {
            craftedItemName = ArticyFieldMapper.GetCraftedItemName(entry);
        }

        public void PopulateIngredientLists(DialogueEntry entry)
        {
            requriedIngredients = ArticyFieldMapper.GetRequiredIngredients(entry);
            missingIngredients = ArticyFieldMapper.GetMissingIngredients(entry);
        }

        public void PopulateApplicableModifiers(DialogueEntry entry)
        {
            applicableModifiers = ArticyFieldMapper.GetApplicableModifiers(entry);
        }
        

        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine($"Check Type: {checkType}");
            result.AppendLine($"Base Difficulty: {baseDifficulty}");
    
            // Required Ingredients
            result.AppendLine("Required Ingredients:");
            if (requriedIngredients != null && requriedIngredients.Count > 0)
            {
                foreach (var ingredient in requriedIngredients)
                {
                    result.AppendLine($"  - {ingredient}");
                }
            }
            else
            {
                result.AppendLine("  None");
            }
    
            // Missing Ingredients
            result.AppendLine("Missing Ingredients:");
            if (missingIngredients != null && missingIngredients.Count > 0)
            {
                foreach (var ingredient in missingIngredients)
                {
                    result.AppendLine($"  - {ingredient}");
                }
            }
            else
            {
                result.AppendLine("  None");
            }
    
            // Applicable Modifiers
            result.AppendLine("Applicable Modifiers:");
            if (applicableModifiers != null && applicableModifiers.Count > 0)
            {
                foreach (var modifier in applicableModifiers)
                {
                    result.AppendLine($"  - {modifier.varName}: {modifier.bonus}");
                }
            }
            else
            {
                result.AppendLine("  None");
            }

            return result.ToString();
        }
        
    }
}