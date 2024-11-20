namespace Contagion.Metric
{
    /// <summary>
    /// Class for translating the Difficulty enum (default difficulties) into the appropriate
    /// difficulty name that will be displayed in dialogue 
    /// </summary>
    public static class DifficultyDisplayMapper
    {
        private static string GetCraftingDifficultyName(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.TRIVIAL:
                    return "Simple Recipe";
                case Difficulty.EASY:
                    return "Standard Recipe";
                case Difficulty.MEDIUM:
                    return "Complex Recipe";
                case Difficulty.CHALLENGING:
                    return "Masterwork Recipe";
                case Difficulty.FORMIDABLE:
                    return "Legendary Recipe";
                default:
                    return "Unknown Difficulty";
            }
        }

        public static string GetDifficultyName(Difficulty difficulty, CheckType checkType)
        {
            switch (checkType)
            {
                case CheckType.CRAFTING:
                    return GetCraftingDifficultyName(difficulty);
                // Add other check types as needed
                default:
                    return difficulty.ToString();
            }
        }
    }
}