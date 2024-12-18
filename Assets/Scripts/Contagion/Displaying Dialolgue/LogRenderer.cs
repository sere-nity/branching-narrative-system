using System;
using System.Collections;
using System.Collections.Generic;
using Contagion;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class LogRenderer : MonoBehaviour
{

    public RectTransform logPanel;
    public LogEntry logTemplate;
    // private List<DialogueEntry> dialogueHistory = new List<DialogueEntry>();
    private static List<FinalEntry> queue = new List<FinalEntry>();
    private LogEntry lastEntry;

    
    public static bool IsBusy => queue.Count > 0;

    public void AddToLog(FinalEntry entry)
    {
        queue.Add(entry);
        ProcessQueue();
    }

    public void Start()
    {
        logTemplate.gameObject.SetActive(false);
    }
    

    private void ProcessQueue()
    {
        if (queue.Count > 0)
        {
            Debug.Log("Processing queue");;
            var entry = queue[0];
            queue.RemoveAt(0);
            
            LogEntry logEntry = UnityEngine.Object.Instantiate(logTemplate);
            lastEntry = logEntry;
            Debug.Log("Instantiated log entry");
            logEntry.transform.SetParent(logPanel, false);
            
            logEntry.Entry = entry;
            Debug.Log($"Log entry created for {entry.spokenLine}");
            
            // start auto scrolling
            SingletonMonoBehaviour<AutoScroller>.Singleton.LogWasUpdated();
        }
    }

    public void ClearLog()
    {
        queue.Clear();
        for (int i = 0; i < logPanel.childCount; i++)
        {
            Transform child = logPanel.GetChild(i);
            // flag is here such that the first log entry is not destroyed when OnConversationStart is called 
            bool isChildLastEntry = child != lastEntry?.transform;
            if (!(child == logTemplate.transform) && isChildLastEntry)
            {
                Destroy(child.gameObject);
            }
           
        }
    }
}