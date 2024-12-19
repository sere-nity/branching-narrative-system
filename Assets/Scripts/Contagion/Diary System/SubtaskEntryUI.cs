using UnityEngine;
using TMPro;
using Contagion.Diary_System;

public class SubtaskEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text subtaskTitle;
    
    private readonly Color activeColor = Color.yellow;
    private readonly Color completedColor = Color.gray;
    private readonly Color cancelledColor = Color.red;
    
    private BaseDiaryTask currentSubtask;
    
    public void SetSubtaskTitle(string title)
    {
        subtaskTitle.text = title;
    }
    
    public void SetSubtask(BaseDiaryTask subtask)
    {
        currentSubtask = subtask;
        subtaskTitle.text = subtask.Name;
        UpdateVisuals();
    }
    
    public void UpdateVisuals()
    {
        if (currentSubtask == null) return;
        
        subtaskTitle.fontStyle = currentSubtask.IsDone ? FontStyles.Strikethrough : FontStyles.Normal;
        
        if (!currentSubtask.IsVisible)
        {
            subtaskTitle.color = cancelledColor;
        }
        else if (currentSubtask.IsDone)
        {
            subtaskTitle.color = completedColor;
        }
        else
        {
            subtaskTitle.color = activeColor;
        }
    }
}