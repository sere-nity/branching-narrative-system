using UnityEngine;

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
    }
}