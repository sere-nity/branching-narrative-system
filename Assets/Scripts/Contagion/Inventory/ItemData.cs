using System.Collections.Generic;
using UnityEngine;
using Contagion.Essence_System;

namespace Contagion.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ItemDetails")]
    public class ItemData : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite icon; 
        public string description;
        // crafted data idk think about this later
        public bool isCrafted;
        // something about it being an ingredient
        public bool isStackable;

        // New fields for alchemy system
        [Header("Alchemy Requirements")]
        public List<EssenceRequirement> essenceRequirements;
        public List<ItemRequirement> craftingIngredients;
        public string memoryConversationName; // this is the name of the memory conversation to play when the item is crafted

        // Helper method to convert to dictionary if needed
        public Dictionary<EssenceType, float> GetEssenceRequirementsDictionary()
        {
            Dictionary<EssenceType, float> requirements = new Dictionary<EssenceType, float>();
            if (essenceRequirements != null)
            {
                foreach (var req in essenceRequirements)
                {
                    requirements[req.essenceType] = req.amount;
                }
            }
            return requirements;
        }
    }

    [System.Serializable]
    public class EssenceRequirement
    {
        public EssenceType essenceType;
        public float amount;
    }

    [System.Serializable]
    public class ItemRequirement
    {
        public ItemData item;
        public int amount;
    }
}