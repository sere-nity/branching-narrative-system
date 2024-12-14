using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion
{
    public class LogEntry : MonoBehaviour
    {
        public SimpleResponseText logText;  // Reuse your SimpleResponseText
        private FinalEntry entry;
    
        
        public FinalEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                HandleLog(entry);
                gameObject.SetActive(true);
            }
        }

        private void HandleLog(FinalEntry finalEntry)
        {
            // Basic text formatting
            string formattedText = $"{finalEntry.speakerName} - {finalEntry.spokenLine}";
        
            // If there's a check, add tooltip components
            if (finalEntry.HasCheck)
            {
                TooltipSource tooltipSource = logText.GetComponentInChildren<TooltipSource>();

                if (!tooltipSource)
                {
                    Debug.LogError("Could not find Tooltip Source component in children for log enrty!");
                }
            
                tooltipSource.tooltipData = new TooltipData { checkResult = finalEntry.checkResult };
                tooltipSource.tooltipType = TooltipType.POST_CRAFTING_CHECK;
            
                // Add check result to displayed text (similar to Disco Elysium's code)
                formattedText += $" [{finalEntry.checkResult.resultType}]";
            }
        
            logText.text = formattedText;
        }
    }
}