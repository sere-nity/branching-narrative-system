using System.Collections.Generic;
using UnityEngine;

namespace Contagion.Diary_System
{
    /// <summary>
    /// Analogous to the JournalModel class from Disco Elysium - acts as a cache or store of data for quests that are represented
    /// by conversations in the dialogue databse. The data is already in the dialogue database, we are just caching it for
    /// efficient lookup. So whenever other systems need to access quest data, they go via this class
    /// </summary>
    public class DiarySystemCache
    {
        // simple singleton pattern. This is the lazy singleton pattern
        // where we only create an instance when this instance is first accessed. 
        // this means we have a private constructor
        public static DiarySystemCache SINGLETON = new DiarySystemCache();
        
        private Dictionary<string, BaseDiaryTask> variableToDiaryTask;
        public HashSet<DiaryTask> AllAvailableDiaryTasks;
        public HashSet<DiaryTask> AddedDiaryTasks;

        private DiarySystemCache()
        {
            variableToDiaryTask = new Dictionary<string, BaseDiaryTask>();
            AllAvailableDiaryTasks = new HashSet<DiaryTask>();
            AddedDiaryTasks = new HashSet<DiaryTask>();
        }
        
        /// <summary>
        /// called when the player starts a task
        /// </summary>
        /// <param name="varName"></param>
        public void AddDiaryTask(string varName)
        {
            BaseDiaryTask diaryTask = GetDiaryTaskByVariable(varName);
            if (diaryTask == null)
            {
                return;
            }
            AddedDiaryTasks.Add(diaryTask as DiaryTask);
        }

        /// <summary>
        /// called when we wanna add a task into this cache
        /// </summary>
        /// <returns></returns>
        public DiaryTask InitializeDiaryTask(string name, string description, string showCondition, string doneCondition, string cancelCondition, int reward)
        {
            DiaryTask diaryTask = new DiaryTask(name, description, showCondition, doneCondition, cancelCondition, reward);
            AllAvailableDiaryTasks.Add(diaryTask);
            AddDiaryTaskToVariableToTaskMap(diaryTask);
            return diaryTask;
        }

        public void AddDiaryTaskToVariableToTaskMap(BaseDiaryTask task)
        {
            // Validate required variables
            if (string.IsNullOrEmpty(task.ShowVariable) || string.IsNullOrEmpty(task.DoneVariable))
            {
                Debug.LogError($"Task '{task.Name}' is missing required ShowVariable or DoneVariable");
                return;
            }

            if (!variableToDiaryTask.ContainsKey(task.ShowVariable))
            {
                AddToDictionary(task.ShowVariable, task);
                AddToDictionary(task.DoneVariable, task);
                if (!string.IsNullOrEmpty(task.CancelVariable))
                {
                    AddToDictionary(task.CancelVariable, task);
                }
            }
            else
            {
                Debug.LogError("Duplicate show condition for tasks: " + task.Name + " | " + variableToDiaryTask[task.ShowVariable].Name);
            }
        }
        
        public DiaryTask GetDiaryTaskByVariable(string varName)
        {
            if (variableToDiaryTask.TryGetValue(varName, out BaseDiaryTask diaryTask))
            {
                return diaryTask as DiaryTask;
            }
            return null;
        }

        private void AddToDictionary(string key, BaseDiaryTask value)
        {
            if (!variableToDiaryTask.ContainsKey(key))
            {
                variableToDiaryTask.Add(key, value);
                return;
            }
            Debug.LogError("Duplicate variable '" + key + "' for tasks " + variableToDiaryTask[key].Name + " and " + value.Name);
        }
    }
}