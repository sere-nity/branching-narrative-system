namespace Contagion.Metric
{
    public class CheckModifier : CheckBonus
    {
        public string varName;
        public CheckModifier(string varName, int bonus, string tooltip) : base(bonus, tooltip)
        {
            this.varName = varName;
        }
    }
}