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
        
        public void SetTask(BaseDiaryTask task)
        {
            taskTitle.text = task.Name;
            taskDescription.text = task.Description;
            
            // Clear existing subtasks
            foreach (Transform child in subtaskContainer)
            {
                if (child.transform != subtaskEntryUI.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            
            if (task is DiaryTask diaryTask)
            {
                // note we're displaying all subtasks here even if we've not "gained" them yet
                foreach (var subtask in diaryTask.AvailableSubtasks)
                {
                    Debug.Log($"Adding Subtask: {subtask.Name}");
                    SubtaskEntryUI subtaskUI = Instantiate(subtaskEntryUI, subtaskContainer);
                    subtaskUI.SetSubtaskTitle(subtask.Name);
                    subtaskUI.gameObject.SetActive(true);
                }
            }
        }
        
        

    }
}