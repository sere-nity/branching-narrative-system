using System.Collections.Generic;
using Contagion.Metric;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion.Lua_Interfacing
{
    /// <summary>
    /// These mappers should map Ids that can be populated in articy fields to actual data
    /// in unity. The id corresponds to the index of the mapper arrays. Why are we doing this?
    /// So we don't have to populate the articy fields with the real data especially if it is long
    /// and complex 
    /// </summary>
    public static class ArticyFieldMapper
    {
        #region Constants

        private const int DIFFICULTY_ARRAY_SIZE = 5;

        private static readonly string[] ingredientFieldList = new string[10]
        {
            "ingredient1", "ingredient2", "ingredient3", "ingredient4", "ingredient5",
            "ingredient6", "ingredient7", "ingredient8", "ingredient9", "ingredient10"
        };

        private static readonly string[] variableFieldList = new string[10]
        {
            "variable1", "variable2", "variable3", "variable4", "variable5",
            "variable6", "variable7", "variable8", "variable9", "variable10"
        };

        private static readonly string[] modifierFieldList = new string[10]
        {
            "modifier1", "modifier2", "modifier3", "modifier4", "modifier5",
            "modifier6", "modifier7", "modifier8", "modifier9", "modifier10"
        };

        private static readonly string[] tooltipFieldList = new string[10]
        {
            "tooltip1", "tooltip2", "tooltip3", "tooltip4", "tooltip5",
            "tooltip6", "tooltip7", "tooltip8", "tooltip9", "tooltip10"
        };

        private const int INGREDIENT_LIST_MAX = 10;
        private const string INVENTORY_PREFIX = "inventory.";

        #endregion

        #region Difficulty Mapping

        private static readonly Difficulty[] ArticyDifficultyIdToDifficulty = new Difficulty[DIFFICULTY_ARRAY_SIZE]
        {
            Difficulty.TRIVIAL,
            Difficulty.EASY,
            Difficulty.MEDIUM,
            Difficulty.CHALLENGING,
            Difficulty.FORMIDABLE,
        };

        /// <summary>
        /// :)
        /// </summary>
        /// <param name="id">This is the difficulty id that populates the articy DifficultyXXX field </param>
        /// <returns>the actual difficulty level as defined by the Difficulty enum. </returns>
        public static int DifficultyIdToThreshold(int id)
        {
            if (id >= ArticyDifficultyIdToDifficulty.Length)
            {
                Debug.LogError($"DifficultyIdToThreshold out of range: {id}");
                return (int)Difficulty.TRIVIAL;
            }

            return (int)ArticyDifficultyIdToDifficulty[id];
        }

        #endregion

        #region Ingredient Mapping

        public static List<string> GetRequiredIngredients(DialogueEntry entry)
        {
            List<string> ingredients = new List<string>();

            for (int i = 0; i < INGREDIENT_LIST_MAX; i++)
            {
                // Gets the string that is stored in the articy field ingredient1-10
                string variableName = Field.LookupValue(entry.fields, ingredientFieldList[i]);

                if (!string.IsNullOrEmpty(variableName))
                {
                    // Extract actual ingredient name from variable name
                    // e.g., "inventory.copper_wire" -> "copper_wire"
                    string ingredientName = variableName.Substring(INVENTORY_PREFIX.Length);
                    ingredients.Add(ingredientName);
                }
            }

            return ingredients;
        }

        public static List<string> GetMissingIngredients(DialogueEntry entry)
        {
            List<string> missingIngredients = new List<string>();

            for (int i = 0; i < INGREDIENT_LIST_MAX; i++)
            {
                // Gets the string that is stored in the articy field ingredient1-10
                string variableName = Field.LookupValue(entry.fields, ingredientFieldList[i]).Trim();

                if (!string.IsNullOrEmpty(variableName))
                {
                    // Check if the Lua variable is false i.e. if the player doesn't 
                    // heve the item in their inventory (indicated by articy variables) 
                    if (!DialogueLua.GetVariable(variableName).AsBool)
                    {
                        string ingredientName = variableName.Substring(INVENTORY_PREFIX.Length);
                        missingIngredients.Add(ingredientName);
                    }
                }
            }

            return missingIngredients;
        }

        #endregion

        #region Modifier Mapping

        /// <summary>
        /// Returns the list of applicable modifiers. These are the lua variables that can affect the outcome of a
        /// check by giving a numerical bonus (stored in modifier1-10). 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static List<CheckModifier> GetApplicableModifiers(DialogueEntry entry)
        {
            Debug.Log("Getting applicable modifiers");
            List<CheckModifier> modifiers = new List<CheckModifier>();

            for (int i = 0; i < 10; i++)
            {
                // First get the condition expression
                string expression = Field.LookupValue(entry.fields, variableFieldList[i]).Trim();
                Debug.Log($"Expression: {expression}");


                if (!string.IsNullOrEmpty(expression))
                {
                    // Access the variable directly with the correct path
                    bool applicable = DialogueLua.GetVariable(expression).AsBool;
                    Debug.Log($"Checking {expression}: {applicable}");

                    if (applicable)
                    {
                        string tooltip = Field.LookupValue(entry.fields, tooltipFieldList[i]);
                        int bonus = Field.LookupInt(entry.fields, modifierFieldList[i]);
                        modifiers.Add(new CheckModifier(expression, bonus, tooltip));
                    }
                }
            }

            return modifiers;
        }

        #endregion

        #region Outcome Mapping

        private static readonly string[] outcomeFieldList = new string[]
        {
            "criticalVariable",
            "successVariable",
            "partialVariable",
            "failureVariable"
        };

        public static List<ResultType> GetAvailableOutcomes(DialogueEntry entry)
        {
            List<ResultType> availableOutcomes = new List<ResultType>();

            // Always add mandatory outcomes
            availableOutcomes.Add(ResultType.SUCCESS);
            availableOutcomes.Add(ResultType.FAILURE);

            // Check for optional outcomes
            if (!string.IsNullOrEmpty(Field.LookupValue(entry.fields, outcomeFieldList[0])))
                availableOutcomes.Add(ResultType.CRITICAL_SUCCESS);

            if (!string.IsNullOrEmpty(Field.LookupValue(entry.fields, outcomeFieldList[2])))
                availableOutcomes.Add(ResultType.PARTIAL_SUCCESS);

            return availableOutcomes;
        }

        public static void SetResultVariable(DialogueEntry entry, ResultType resultType)
        {
            int index = (int)resultType;
            string variableName = Field.LookupValue(entry.fields, outcomeFieldList[index]);
            if (!string.IsNullOrEmpty(variableName))
            {
                DialogueLua.SetVariable(variableName, true);
                Debug.Log("Set " + variableName + " to true");
            }
        }

        #endregion

        // Add other mapping sections as needed

        #region CraftedItem Mapping

        private static readonly string[] craftedItemFields = new string[]
        {
            "craftedItemName",
        };
        
        public static string GetCraftedItemName(DialogueEntry entry)
        {
            return Field.LookupValue(entry.fields, craftedItemFields[0]);
        }


        #endregion
    }

   
}