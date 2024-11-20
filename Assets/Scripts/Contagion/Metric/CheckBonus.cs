namespace Contagion.Metric
{
    public class CheckBonus
    {
        public int bonus;

        public string explanation;

        public bool IsPenalty
        {
            get
            {
                return bonus < 0;
            }
        }

        public CheckBonus(int bonus, string explanation)
        {
            this.bonus = bonus;
            this.explanation = explanation;
        }

        public override string ToString()
        {
            return string.Format("[CheckBonus: bonus={0}; explanation={1}]", bonus, explanation);
        }
    }
}