using UnityEngine;
using System.Collections.Generic;

namespace Contagion.Diary_System
{
    public class DiaryUI : MonoBehaviour
    {
        [SerializeField] private DiaryEntryUI diaryEntryUI;
        [SerializeField] private Transform diaryEntryParent;
        
        private Dictionary<string, DiaryEntryUI> taskUIItems = new Dictionary<string, DiaryEntryUI>();

        private void OnEnable()
        {
            RefreshDiaryEntries(DiarySystemCache.SINGLETON.AllAvailableDiaryTasks);
        }

        private void OnDisable()
        {
            // Clear existing entries when disabled
            foreach (var ui in taskUIItems.Values)
            {
                if (ui != null && ui.transform != diaryEntryUI.transform)
                    Destroy(ui.gameObject);
            }
            taskUIItems.Clear();
        }

        public void RefreshDiaryEntries(IEnumerable<BaseDiaryTask> tasks)
        {
            // Clear existing
            foreach (var ui in taskUIItems.Values)
            {
                if (ui != null && ui.transform != diaryEntryUI.transform)
                    Destroy(ui.gameObject);
            }
            taskUIItems.Clear();

            // Create new
            foreach (var task in tasks)
            {
                DiaryEntryUI entryUI = Instantiate(diaryEntryUI, diaryEntryParent);
                entryUI.SetTask(task);
                entryUI.gameObject.SetActive(true);
                taskUIItems.Add(task.Name, entryUI);
            }
        }
    }
}