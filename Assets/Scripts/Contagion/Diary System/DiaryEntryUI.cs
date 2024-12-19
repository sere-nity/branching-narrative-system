using TMPro;
using UnityEngine;

namespace Contagion.Diary_System
{
    public class DiaryEntryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text taskTitle;
        [SerializeField] private TMP_Text taskDescription;
        [SerializeField] private Transform subtaskContainer;
        [SerializeField] private SubtaskEntryUI subtaskEntryUI;
        
        private readonly Color activeColor = Color.yellow;
        private readonly Color completedColor = Color.gray;
        private readonly Color cancelledColor = Color.red;
        
        private BaseDiaryTask currentTask;
        
        public void SetTask(BaseDiaryTask task)
        {
            currentTask = task;
            taskTitle.text = task.Name;
            taskDescription.text = task.Description;
            
            // Initial setup of subtasks if not already setup
            if (task is DiaryTask diaryTask && subtaskContainer.childCount <= 1)
            {
                SetupSubtasks(diaryTask);
            }
            
            UpdateVisuals();
        }
        
        public void UpdateVisuals()
        {
            if (currentTask == null) return;
            
            Debug.Log($"Task '{currentTask.Name}' states - IsVisible: {currentTask.IsVisible}, IsDone: {currentTask.IsDone}");
            
            // Always set strikethrough based on done state
            taskTitle.fontStyle = currentTask.IsDone ? FontStyles.Strikethrough : FontStyles.Normal;
            
            // Set colors based on state priority
            if (!currentTask.IsVisible)
            {
                Debug.Log($"Task '{currentTask.Name}' is not visible - setting red");
                taskTitle.color = cancelledColor;
                taskDescription.color = cancelledColor;
            }
            else if (currentTask.IsDone)
            {
                Debug.Log($"Task '{currentTask.Name}' is done - setting gray");
                taskTitle.color = completedColor;
                taskDescription.color = completedColor;
            }
            else
            {
                Debug.Log($"Task '{currentTask.Name}' is active - setting yellow");
                taskTitle.color = activeColor;
                taskDescription.color = Color.white;
            }
            
            // Update subtask visuals
            if (currentTask is DiaryTask diaryTask)
            {
                foreach (Transform child in subtaskContainer)
                {
                    var subtaskUI = child.GetComponent<SubtaskEntryUI>();
                    if (subtaskUI != null && subtaskUI != subtaskEntryUI)
                    {
                        subtaskUI.UpdateVisuals();
                    }
                }
            }
        }
        
        private void SetupSubtasks(DiaryTask diaryTask)
        {
            foreach (var subtask in diaryTask.AvailableSubtasks)
            {
                SubtaskEntryUI subtaskUI = Instantiate(subtaskEntryUI, subtaskContainer);
                subtaskUI.SetSubtaskTitle(subtask.Name);
                subtaskUI.gameObject.SetActive(true);
            }
        }
    }
}