using Contagion.Metric;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Contagion
{
    public class ModifierEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text modifierNameText;
        [SerializeField] private TMP_Text modifierValueText;
        [SerializeField] private TMP_Text modifierDescriptionText;
        
        /// <summary>
        /// Populates the modifier template with the modifier's name and value and sets the object to active
        /// </summary>
        /// <param name="modifier"></param>
        public void SetModifier(CheckModifier modifier)
        { 
            string bonusText = modifier.bonus > 0 ? $"+{modifier.bonus}" : modifier.bonus.ToString();
            modifierValueText.text = bonusText;
            modifierNameText.text = modifier.varName;
            modifierDescriptionText.text = modifier.explanation;
            gameObject.SetActive(true);
        }
    }
}