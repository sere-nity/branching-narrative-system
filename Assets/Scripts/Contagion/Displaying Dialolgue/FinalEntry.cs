using Contagion.Metric;
using PixelCrushers.DialogueSystem;

namespace Contagion
{
    public class FinalEntry
    {
        public DialogueEntry entry;
        public CheckResult checkResult;
        public string speakerName;
        public string spokenLine;
    
        public bool HasCheck => checkResult != null;

        public FinalEntry(DialogueEntry entry)
        {
            this.entry = entry;
            this.speakerName = DialogueManager.MasterDatabase.GetActor(entry.ActorID).Name;
            this.spokenLine = entry.subtitleText;
        }
    }
}