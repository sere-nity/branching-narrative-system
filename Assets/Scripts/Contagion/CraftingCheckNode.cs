using System;
using Contagion;
using Contagion.Lua_Interfacing;
using Contagion.Metric;
using PixelCrushers.DialogueSystem;
using UnityEngine;


public static class CraftingCheckNode
{
    public const string DIFFICULTY_ARTICY_ID_FIELD = "DifficultyCrafting";
    // TODO - is it worth having a flag name to keep track of whether we've attempted 
    // it or not? e.g. candle_making.attempt_melt_tallow 
    public const string FLAG_NAME_FIELD = "FlagName";

    /// <summary>
    /// Populates the fields of the Check Result class and returns it 
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static CheckResult GetCheckResult(DialogueEntry entry)
    {
        CheckResult checkResult = new CheckResult(CheckType.CRAFTING);
        
        // set base difficulty field based on the id defined in articy which
        // references a difficulty level 
        checkResult.baseDifficulty = ArticyFieldMapper.DifficultyIdToThreshold(
            Field.LookupInt(entry.fields, DIFFICULTY_ARTICY_ID_FIELD)
        );
        
        // populate ingredient lists (required and missing)
        // TODO - maybe add tooltip functionality later
        checkResult.PopulateIngredientLists(entry);
        
        // populate applicable modifiers list 
        checkResult.PopulateApplicableModifiers(entry);
        
        // populate crafted item info for the pre-check crafting response tooltip 
        checkResult.PopulateCraftedItemInfo(entry);
    
        // Set locked state based on missing ingredients
        // checkResult.isLocked = checkResult.missingIngredients.Count > 0;
        
        return checkResult;
    }

    // This method should do the actual calculation of the check ...? 
    public static CheckResult CheckSuccess(DialogueEntry entry)
    {
        CheckResult checkResult = GetCheckResult(entry);
        
        // Calculate result (modifies checkResult directly)
        CraftingResultCalculator.CalculateResult(entry, checkResult);
    
        // Handle aftermath based on result
        HandleResultAftermath(checkResult);

        return checkResult;
    }

    private static void HandleResultAftermath(CheckResult checkResult)
    {
        Debug.Log(checkResult.ToString());
        // throw new NotImplementedException();
    }

    public static bool IsCraftingCheck(DialogueEntry entry)
    {
        return Field.FieldExists(entry.fields, DIFFICULTY_ARTICY_ID_FIELD);
    }
    
    // Returns the response text that is the next dialogue entry (not the 
    // current response) 
    public static FinalResponseText HandleResponseText(Response response)
    {
        DialogueEntry destinationEntry = response.destinationEntry;
        string responseText = destinationEntry.subtitleText;
        FinalResponseText finalResponseText = new FinalResponseText(response, responseText);
        // TODO - SPECIFIC CRAFTING CHECK FUNCTIONALITY - like getting Check Text - which I'm not sure what this is atm
        return finalResponseText;
    }
}
