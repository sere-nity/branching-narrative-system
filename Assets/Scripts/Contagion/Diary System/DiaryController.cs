using System.Collections.Generic;
using Contagion.Lua_Interfacing;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Contagion.Diary_System
{
    public class DiaryController : SingletonMonoBehaviour<DiaryController>
    {
        [SerializeField] private DiaryUI diaryUI;

        private void Start()
        {
            ImportFromArticy();
            DiaryWatcher.CheckForChanges(true);
            RefreshUI();
        }

        public static void ImportFromArticy()
        {
            ArticyDiaryImporter.Import();
        }

        private void RefreshUI()
        {
            diaryUI.RefreshDiaryEntries(DiarySystemCache.SINGLETON.AllAvailableDiaryTasks);
        }

        public void ToggleDiary()
        {
            if (diaryUI.gameObject != null)
            {
                bool willBeActive = !diaryUI.gameObject.activeSelf;
                diaryUI.gameObject.SetActive(willBeActive);
                
                if (willBeActive)
                {
                    DiaryWatcher.CheckForChanges(false);
                    diaryUI.UpdateVisuals();
                }
            }
        }

        // Add this to DiaryController for testing
        [ContextMenu("Test Task States")]
        private void TestTaskStates()
        {
            // Create test task with different states
            var task = DiarySystemCache.SINGLETON.InitializeDiaryTask(
                "Test Task", 
                "Test Description",
                "MainTask_Show",
                "MainTask_Done",
                "MainTask_Cancel",
                100
            );
            
            // Add a test subtask with different variable names
            if (task is DiaryTask diaryTask)
            {
                diaryTask.AddSubtask(
                    "Test Subtask", 
                    "Subtask Description", 
                    "SubTask_Show",
                    "SubTask_Done", 
                    "SubTask_Cancel", 
                    false
                );
            }

            // Set initial state through Lua variable
            DialogueLua.SetVariable("MainTask_Show", true);
            DiaryWatcher.CheckForChanges(true);  // Let DiaryWatcher handle the state change
            RefreshUI();
            
            Debug.Log("Task states set. Open diary to see yellow (active) task");
        }

        [ContextMenu("Mark Task Done")]
        private void MarkTaskDone()
        {
            DialogueLua.SetVariable("MainTask_Done", true);
            DiaryWatcher.CheckForChanges(true);
            Debug.Log("Task marked as done. Open diary to see gray strikethrough");
        }

        [ContextMenu("Cancel Task")]
        private void CancelTask()
        {
            DialogueLua.SetVariable("MainTask_Cancel", true);
            DialogueLua.SetVariable("MainTask_Show", false);
            DiaryWatcher.CheckForChanges(true);
            Debug.Log("Task cancelled. Open diary to see red text");
        }
    }
}