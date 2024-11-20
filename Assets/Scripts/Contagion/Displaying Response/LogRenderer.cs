using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion
{
    public class LogRenderer : MonoBehaviour
    {
        public RectTransform logPanel;          // Parent for log entries
        public LogEntry logTemplate;            // Template for instantiating entries
        private List<DialogueEntry> dialogueHistory = new List<DialogueEntry>();
    
        public void AddToLog(DialogueEntry entry)
        {
            // Create new log entry
            LogEntry logEntry = Instantiate(logTemplate);
            logEntry.transform.SetParent(logPanel, false);
        
            // Set up entry
            logEntry.Entry = entry;
            dialogueHistory.Add(entry);
        }

        public void ClearLog()
        {
            dialogueHistory.Clear();
            // Clear UI elements except template
            for (int i = 0; i < logPanel.childCount; i++)
            {
                Transform child = logPanel.GetChild(i);
                if (child != logTemplate.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}