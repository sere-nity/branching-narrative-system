using UnityEngine;

namespace Contagion.Memory_System
{
    public class MemoryManager : MonoBehaviour
    {
        public event System.Action<string> OnMemoryTriggered;
    
        public void TriggerMemory(string memoryText) {
            OnMemoryTriggered?.Invoke(memoryText);
            // Implement memory visualization/dialogue system here
        }
    }
}