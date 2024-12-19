using System;
using Contagion.Lua_Interfacing;
using UnityEngine;

namespace Contagion.Diary_System
{
    public abstract class BaseDiaryTask
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public virtual bool IsVisible { get; set; }
        public bool IsDone { get; set; }
        public DateTime AcquisitionTime { get; protected set; }

        public string ShowVariable { get; protected set; }    // Lua/dialogue variable that shows task
        public string DoneVariable { get; protected set; }    // Lua/dialogue variable that completes task
        public string CancelVariable { get; private set; }

        


        protected BaseDiaryTask(string name, string description, string showVariable, string doneVariable, string cancelVariable)
        {
            Name = name;
            Description = description;
            Debug.Log($"showVariable: {showVariable}");
            ShowVariable = ArticyUtils.GetVariableFromLuaExpression(showVariable);
            Debug.Log($"ShowVariable: {ShowVariable}");
            DoneVariable = ArticyUtils.GetVariableFromLuaExpression(doneVariable);
            CancelVariable = ArticyUtils.GetVariableFromLuaExpression(cancelVariable);
            IsVisible = false;
            IsDone = false;
        }

        public virtual void Reveal()
        {
            IsVisible = true;
            AcquisitionTime = DateTime.Now;
        }

        public virtual void MarkDone()
        {
            IsDone = true;
        }
    }
}