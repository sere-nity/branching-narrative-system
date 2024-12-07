using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion
{
    public class LogEntry : MonoBehaviour
    {
        public SimpleResponseText logText;  // Reuse your SimpleResponseText
        private DialogueEntry entry;
    
        public DialogueEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                HandleLog(entry);
                gameObject.SetActive(true);
            }
        }

        private void HandleLog(DialogueEntry dialogueEntry)
        {
            string speakerName = DialogueManager.MasterDatabase.GetActor(dialogueEntry.ActorID).Name;
            string spokenLine = dialogueEntry.subtitleText;
        
            // Format: "SPEAKER - Dialogue text"
            Debug.Log("Setting text to: " + $"{speakerName} - {spokenLine}");
            logText.text = $"{speakerName} - {spokenLine}";
        }
    }
}