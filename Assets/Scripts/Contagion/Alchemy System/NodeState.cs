namespace Contagion.Alchemy_System
{
    public enum NodeState
    {
        Locked,          // Initial state, not yet available
        Progressing,     // Actively gathering essence
        Unlockable,      // Has enough essence, ready to be unlocked
        Unlocked         // Fully unlocked, memory revealed and item craftable
    }
}