using System;
using System.Collections.Generic;
using System.Linq;

namespace Contagion.Essence_System
{
    public class EssenceManager : SingletonMonoBehaviour<EssenceManager>
    {
        // Global essence levels
        private Dictionary<EssenceType, float> globalEssenceLevels = new Dictionary<EssenceType, float>();
        
        // Recent modifiers: EssenceType -> List of (timestamp, variableName, value)
        private Dictionary<EssenceType, List<(DateTime timestamp, string variable, int value)>> recentModifiers 
            = new Dictionary<EssenceType, List<(DateTime, string, int)>>();
        
        private const int MAX_RECENT_MODIFIERS = 5;
        public event Action<EssenceType, float> OnEssenceChanged;

        protected override void Awake()
        {
            base.Awake();
            // Initialize all essence types to 0
            foreach (EssenceType essenceType in System.Enum.GetValues(typeof(EssenceType)))
            {
                globalEssenceLevels[essenceType] = 0f;
            }
        }
        public void AddEssence(EssenceType essenceType, float amount)
        {
            if (!globalEssenceLevels.ContainsKey(essenceType))
                globalEssenceLevels[essenceType] = 0;
                
            globalEssenceLevels[essenceType] += amount;
            OnEssenceChanged?.Invoke(essenceType, globalEssenceLevels[essenceType]);
        }

        public void RecordModifier(string variableName, int modifierValue)
        {
            string essenceTypeStr = ExtractEssenceType(variableName);
            if (string.IsNullOrEmpty(essenceTypeStr)) return;

            // Parse string to enum
            if (Enum.TryParse<EssenceType>(essenceTypeStr, out EssenceType essenceType))
            {
                // Add to global essence
                AddEssence(essenceType, modifierValue);

                // Record modifier
                if (!recentModifiers.ContainsKey(essenceType))
                    recentModifiers[essenceType] = new List<(DateTime, string, int)>();

                var modifierList = recentModifiers[essenceType];
                modifierList.Add((DateTime.Now, variableName, modifierValue));
                
                // Keep only most recent
                if (modifierList.Count > MAX_RECENT_MODIFIERS)
                    modifierList.RemoveAt(0);
            }
        }

        public float GetEssenceLevel(EssenceType essenceType)
        {
            return globalEssenceLevels.ContainsKey(essenceType) ? globalEssenceLevels[essenceType] : 0f;
        }

        public List<(string variable, int value)> GetRecentModifiers(EssenceType essenceType)
        {
            if (!recentModifiers.ContainsKey(essenceType))
                return new List<(string, int)>();

            return recentModifiers[essenceType]
                .OrderByDescending(x => x.timestamp)
                .Select(x => (x.variable, x.value))
                .ToList();
        }

        private string ExtractEssenceType(string variableName)
        {
            // Extract essence type from variable prefix (e.g., "PURIFICATION.something")
            int dotIndex = variableName.IndexOf('.');
            return dotIndex > 0 ? variableName.Substring(0, dotIndex) : string.Empty;
        }

        
    }
}