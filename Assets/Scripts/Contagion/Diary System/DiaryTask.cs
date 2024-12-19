using System.Collections.Generic;
using UnityEngine;

namespace Contagion.Diary_System
{
    public class DiaryTask :  BaseDiaryTask
    {
        public List<DiarySubTask> AvailableSubtasks { get; private set; }
        public List<DiarySubTask> GainedSubtasks { get; private set; }
        public int Reward { get; private set; }
        
        public DiaryTask(string name, string description, string showVariable, string doneVariable, string cancelVariable, int reward) 
            : base(name, description, showVariable, doneVariable, cancelVariable)
        {
            AvailableSubtasks = new List<DiarySubTask>();
            GainedSubtasks = new List<DiarySubTask>();
            Reward = reward;
        }

        public void InitializeSubTask()
        {
            throw new System.NotImplementedException();
        }
        
        public void GainSubtask(DiarySubTask subtask)
        {
            if (!GainedSubtasks.Contains(subtask))
            {
                GainedSubtasks.Add(subtask);
            }
        }

        public void AddSubtask(string name, string description, string showCondition, string doneCondition, string cancelCondition, bool isTimed)
        {
            DiarySubTask subtask = new DiarySubTask(name, "", showCondition, doneCondition, cancelCondition, isTimed);
            
            subtask.parent = this;
            DiarySystemCache.SINGLETON.AddDiaryTaskToVariableToTaskMap(subtask);
            
            AvailableSubtasks.Add(subtask);
            Debug.Log($"Added Subtask: {subtask.Name}");
        }
    }
}