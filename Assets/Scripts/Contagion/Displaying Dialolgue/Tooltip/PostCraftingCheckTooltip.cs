using System.Collections.Generic;
using Contagion.Metric;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    public class PostCraftingCheckTooltip : BaseTooltip<PostCraftingCheckTooltip>
    {
        [Header("Visual ELements")]
        [SerializeField] private TMP_Text resultTypeText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Transform modifierListParent; 
        [SerializeField] private ModifierEntry modifierEntryPrefab;
        private List<ModifierEntry> appliedModifiers = new List<ModifierEntry>();


        public override void SetTooltipContent(TooltipSource tooltipSource)
        {
            var checkResult = tooltipSource.tooltipData.checkResult;
        
            // Set result type and score
            resultTypeText.text = checkResult.resultType.ToString();
            scoreText.text = CraftingResultCalculator.CalculatePercentageSuccess(checkResult).ToString("P0");
            
            UpdateModifierList(checkResult);
            
        }
        
        
        private void UpdateModifierList(CheckResult checkResult)
        {
            // Clear existing entries
            foreach (var entry in appliedModifiers)
            {
                Destroy(entry.gameObject);
            }
            appliedModifiers.Clear();

            // Create new entries for applied modifiers
            foreach (var modifier in checkResult.applicableModifiers)
            {
                var entry = Instantiate(modifierEntryPrefab, modifierListParent);
                entry.SetModifier(modifier);
                appliedModifiers.Add(entry);
            }
        }
        
        

        
    }
}