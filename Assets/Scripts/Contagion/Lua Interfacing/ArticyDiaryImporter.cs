using System.Collections.Generic;
using Contagion.Diary_System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion.Lua_Interfacing
{
    public static class ArticyDiaryImporter
    {   
        /// <summary>
        ///  This should set off the import process for the diary system.
        /// </summary>
        public static void Import()
        {
            ImportTasks(DialogueManager.MasterDatabase.conversations);
        }
        
        public static void ImportTasks(List<Conversation> conversations)
        {
            foreach (Conversation conversation in conversations)
            {
                if (IsQuest(conversation.fields))
                {
                    DiaryTask diaryTask = ImportDiaryTaskFromFields(conversation.fields);
                    
                    ImportSubtaskIntoDiaryTask(diaryTask, conversation.fields);
                }
            }
        }

        private static void ImportSubtaskIntoDiaryTask(DiaryTask diaryTask, List<Field> fields)
        {
            // extract all subtasks here?
            for (int i = 1; i <= StaticConsts.MAX_NR_SUBTASKS; i++)
            {
                string text2 = ((i < 10) ? ("0" + i) : i.ToString());

                Debug.Log($"Checking if subtask {text2} exists");
                Debug.Log("done_subtask_condition_" + text2);
                Debug.Log($"Field: {Field.IsFieldAssigned(fields, "done_subtask_condition_" + text2)}");
                // makes sure we don't add any subtasks that don't exist
                if (!Field.IsFieldAssigned(fields, "done_subtask_condition_" + text2))
				{
					break;
				}

                string name = Field.LookupValue(fields, "subtask_title_" + text2);
                string showCondition = Field.LookupValue(fields, "subtask_condition_" + text2);
                string doneCondition = Field.LookupValue(fields, "done_subtask_condition_" + text2);
                string cancelCondition = Field.LookupValue(fields, "cancel_subtask_condition_" + text2);
                bool isTimed = Field.LookupBool(fields, "timed_subtask_" + i);
                // create a new subtask
                diaryTask.AddSubtask(name, "", showCondition, doneCondition, cancelCondition, isTimed);   
                
            }
        }

        private static DiaryTask ImportDiaryTaskFromFields(List<Field> fields)
        {
            // extract all fields here? 
            string name = Field.LookupValue(fields, "Title");
            string description = Field.LookupValue(fields, "Description");
            string showCondition = Field.LookupValue(fields, "main_task_condition");
            string doneCondition = Field.LookupValue(fields, "done_task_condition");
            string cancelCondition = Field.LookupValue(fields, "cancel_task_condition");
            int reward = Field.LookupInt(fields, "task_reward");
                    
            // create a new diary task
            DiaryTask diaryTask = DiarySystemCache.SINGLETON.InitializeDiaryTask(name, description, showCondition, doneCondition, cancelCondition, reward);

            return diaryTask;
        }

        public static bool IsQuest(List<Field> fields)
        {
            return Field.IsFieldAssigned(fields, "main_task_condition");
        }
        
    }
}