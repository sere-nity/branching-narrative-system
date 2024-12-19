using System.Collections.Generic;
using Contagion.Lua_Interfacing;
using UnityEngine;

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
                diaryUI.gameObject.SetActive(!diaryUI.gameObject.activeSelf);
            }
        }
    }
}