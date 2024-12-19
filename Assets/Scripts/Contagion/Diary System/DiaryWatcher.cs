using UnityEngine;
using PixelCrushers.DialogueSystem;
using Contagion.Lua_Interfacing;

namespace Contagion.Diary_System
{
    public static class DiaryWatcher
    {
        /// <summary>
        /// Checks for changes in the diary tasks and updates the cache accordingly.
        /// Note the afterLoading parameter is used to determine if the changes are due to loading a save or not.
        /// This bool is important to account for any discrepancies in the diary tasks between the Articy project and the game.
        /// The Lua Variables are treated as the source of truth for the diary tasks.
        /// </summary>
        /// <param name="afterLoading"></param>
        public static void CheckForChanges(bool afterLoading)
        {
            int newTaskCount = 0;
            foreach (DiaryTask task in DiarySystemCache.SINGLETON.AllAvailableDiaryTasks)
            {
                foreach (DiarySubTask subtask in task.AvailableSubtasks)
                {
                    DoChecksForTask(subtask, afterLoading);
                }
                
                if (DoChecksForTask(task, afterLoading))
                {
                    newTaskCount++;
                }
            }
            // DiarySystemCache.SINGLETON.NewTaskCount = newTaskCount;
        }

        private static bool DoChecksForTask(BaseDiaryTask task, bool afterLoading)
        {
            CheckVisibility(task, afterLoading);
            CheckDone(task, afterLoading);
            CheckCancelled(task, afterLoading);

            if (task is DiaryTask diaryTask && task.IsVisible && !task.IsDone)
            {
                return true;
            }
            return false;
        }

        private static void CheckVisibility(BaseDiaryTask task, bool afterLoading)
        {
            bool luaIsVisible = DialogueLua.GetVariable(task.ShowVariable).AsBool;
            bool currentlyVisible = task.IsVisible;

            if (afterLoading)
            {
                if (currentlyVisible && !luaIsVisible)
                {
                    task.IsVisible = false;
                }
                else if (!currentlyVisible && luaIsVisible)
                {
                    task.Reveal();
                }
            }
            else if (currentlyVisible != luaIsVisible)
            {
                Debug.LogWarning($"Task visibility mismatch: '{task.Name}' (ShowVariable: {task.ShowVariable}) - Expected: {luaIsVisible}, Current: {currentlyVisible}");
            }
        }

        private static void CheckDone(BaseDiaryTask task, bool afterLoading)
        {
            bool luaIsDone = DialogueLua.GetVariable(task.DoneVariable).AsBool;
            
            if (afterLoading)
            {
                if (task.IsDone && !luaIsDone)
                {
                    task.IsDone = false;
                }
                else if (!task.IsDone && luaIsDone)
                {
                    task.MarkDone();
                }
            }
            else if (task.IsDone != luaIsDone)
            {
                Debug.LogWarning($"Task completion mismatch: '{task.Name}' (DoneVariable: {task.DoneVariable}) - Expected: {luaIsDone}, Current: {task.IsDone}");
            }
        }

        private static void CheckCancelled(BaseDiaryTask task, bool afterLoading)
        {
            if (string.IsNullOrEmpty(task.CancelVariable)) return;

            bool luaIsCancelled = DialogueLua.GetVariable(task.CancelVariable).AsBool;
            
            if (afterLoading)
            {
                // You may want to add IsCancelled property to BaseDiaryTask
                // For now, we'll just update the visibility
                if (luaIsCancelled)
                {
                    task.IsVisible = false;
                }
            }
            else if (luaIsCancelled && task.IsVisible)
            {
                Debug.LogWarning($"Task cancellation mismatch: '{task.Name}' (CancelVariable: {task.CancelVariable}) should not be visible");
            }
        }
    }
}