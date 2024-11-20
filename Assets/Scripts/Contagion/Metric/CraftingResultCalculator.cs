using System;
using System.Collections.Generic;
using Contagion.Lua_Interfacing;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Contagion.Metric
{
    public static class CraftingResultCalculator
    {
        #region constants

        private static readonly string[] OUTCOME_FIELD_NAMES = new string[]
        {
            "criticalVariable",
            "successVariable",
            "partialVariable",
            "failureVariable"
        };
        
        private const int CRITICAL_THRESHOLD = 5;
        private const int PARTIAL_THRESHOLD = -2;
        #endregion
        
        public static void CalculateResult(DialogueEntry entry, CheckResult checkResult)
        {
            // Calculate base value with lua variable modifiers + scaled based on the 
            // number of ingredients the player has 
            int finalValue = CalculateFinalValue(checkResult);
        
            float randomFactor = Random.Range(0.8f, 1.2f);
            finalValue = Mathf.RoundToInt(finalValue * randomFactor);
            
            int difference = finalValue - checkResult.baseDifficulty;
            // gets the possible outcomes based on whether the outcome fields are populated 
            // in articy - we should always have a success and failure outcome 
            var availableOutcomes = ArticyFieldMapper.GetAvailableOutcomes(entry);
        
            checkResult.resultType = DetermineResultType(difference, availableOutcomes);
        }

        private static ResultType DetermineResultType(int difference, List<ResultType> availableOutcomes)
        {
            if (availableOutcomes.Contains(ResultType.CRITICAL_SUCCESS) && difference >= CRITICAL_THRESHOLD)
                return ResultType.CRITICAL_SUCCESS;
            
            if (difference >= 0)
                return ResultType.SUCCESS;
            
            if (availableOutcomes.Contains(ResultType.PARTIAL_SUCCESS) && difference >= PARTIAL_THRESHOLD)
                return ResultType.PARTIAL_SUCCESS;
            
            return ResultType.FAILURE;
        }

        private static int CalculateFinalValue(CheckResult checkResult)
        {
            // Start with base difficulty
            int finalValue = checkResult.baseDifficulty;
    
            // Add conditional modifiers (from variable1-10/modifier1-10)
            foreach (var modifier in checkResult.applicableModifiers)
            {
                finalValue += modifier.bonus;
            }
    
            // Calculate ingredient-based modifier
            float ingredientModifier = CalculateIngredientModifier(
                checkResult.requriedIngredients.Count,
                checkResult.missingIngredients.Count
            );
    
            // Apply ingredient modifier
            finalValue = Mathf.RoundToInt(finalValue * ingredientModifier);
    
            return finalValue;
        }

        private static float CalculateIngredientModifier(int required, int missing)
        {
            if (required == 0) return 1f;
    
            float percentageHave = (float)(required - missing) / required;
    
            // Example scaling:
            // All ingredients = 1.2x multiplier
            // Most ingredients = 1.0x multiplier
            // Few ingredients = 0.8x multiplier
            // No ingredients = 0.6x multiplier
    
            if (percentageHave >= 1f) return 1.2f;
            if (percentageHave >= 0.7f) return 1.0f;
            if (percentageHave >= 0.3f) return 0.8f;
            return 0.6f;
        }

    }
}