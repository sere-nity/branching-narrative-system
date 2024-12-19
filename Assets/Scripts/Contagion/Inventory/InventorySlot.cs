using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private Image icon; 
        
        public void SetItemSlot(string name, Sprite icon)
        {
            itemName.text = name;
            this.icon.sprite = icon;
        }
    }
}