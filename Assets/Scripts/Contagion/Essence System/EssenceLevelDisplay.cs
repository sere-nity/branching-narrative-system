using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Contagion.Essence_System
{
    public class EssenceLevelDisplay : MonoBehaviour
    {
        private Dictionary<EssenceType, EssenceBar> essenceBars;

        private void Awake()
        {
            // Find all EssenceBar components in children
            essenceBars = GetComponentsInChildren<EssenceBar>()
                .ToDictionary(bar => bar.essenceType);
        }
        private void Start()
        {
            // links this UI to the essence manager
            EssenceManager.Singleton.OnEssenceChanged += UpdateEssenceBars;
            foreach (EssenceType essenceType in Enum.GetValues(typeof(EssenceType)))
            {
                UpdateEssenceBars(essenceType, 0f);
            }
        }

        private void UpdateEssenceBars(EssenceType essenceType, float newLevel)
        {
            if (essenceBars.TryGetValue(essenceType, out EssenceBar bar))
            {
                bar.UpdateValue(newLevel);
            }
        }
        
        
    }
}