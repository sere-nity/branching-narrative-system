using System.Collections.Generic;
using Contagion.Metric;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    public class PreCraftingCheckTooltip : BaseTooltip<PreCraftingCheckTooltip>
    {
        [Header("Visual ELements")]
        [SerializeField] private Image blackAndWhiteImage;
        [SerializeField] private Image colorImage;
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private Transform ingredientListParent; 
        [SerializeField] private IngredientEntry ingredientEntryPrefab;
        private List<IngredientEntry> spawnedIngredients = new List<IngredientEntry>();
        private Sprite defaultItemSprite;


        public override void SetTooltipContent(TooltipSource tooltipSource)
        {
            var checkResult = tooltipSource.tooltipData.checkResult;
        
            // Set item preview and name
            Sprite craftedItemSprite = GetItemSprite(checkResult);
            blackAndWhiteImage.sprite = craftedItemSprite;
            colorImage.sprite = craftedItemSprite;
            itemNameText.text = GetItemName(checkResult);

            // Calculate and set success chance
            float successChance = CraftingResultCalculator.CalculatePercentageSuccess(checkResult);
            colorImage.fillAmount = successChance;

            // Update ingredient list
            UpdateIngredientList(checkResult);
        }

        private Sprite GetItemSprite(CheckResult checkResult)
        {   
            string spritePath = $"Sprites/Items/{checkResult.craftedItemName}";
            Sprite sprite = Resources.Load<Sprite>(spritePath);
    
            if (sprite == null)
            {
                Debug.LogWarning($"Missing sprite for item: {checkResult.craftedItemName}");
                return defaultItemSprite; // Fallback sprite
            }
    
            return sprite;
        }

        private string GetItemName(CheckResult checkResult)
        {
            // TODO - is the name of this the actual name we want to display?
            // or will it be the name of the inventory variable like inventory.crafted_item_name
            return checkResult.craftedItemName;
        }


        private void UpdateIngredientList(CheckResult checkResult)
        {
            // Clear existing entries
            foreach (var entry in spawnedIngredients)
            {
                Destroy(entry.gameObject);
            }
            spawnedIngredients.Clear();

            // Create new entries for required ingredients
            foreach (var ingredient in checkResult.requriedIngredients)
            {
                var entry = Instantiate(ingredientEntryPrefab, ingredientListParent);
                bool hasIngredient = !checkResult.missingIngredients.Contains(ingredient);
                entry.SetIngredient(ingredient, hasIngredient);
                Debug.Log("Ingredient: " + ingredient + " hasIngredient: " + hasIngredient);
                spawnedIngredients.Add(entry);
            }
        }
        

    }
}