namespace Contagion.Diary_System
{
    public class DiarySubTask : BaseDiaryTask
    {
        public DiaryTask parent;

        public override bool IsVisible
        {
            get
            {
                if (base.IsVisible)
                {
                    return parent.IsVisible;
                }
                return false;
            }
        }
        public DiarySubTask(string name, string description, string showVariable, string doneVariable, string cancelVariable, bool isTimed) 
            : base(name, description, showVariable, doneVariable, cancelVariable)
        {
            
        }
    }
}