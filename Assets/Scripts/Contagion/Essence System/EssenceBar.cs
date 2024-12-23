using UnityEngine;
using UnityEngine.UI;

namespace Contagion.Essence_System
{
    public class EssenceBar : MonoBehaviour
    {
        public EssenceType essenceType;
        public Slider progressBar;
        public TMPro.TextMeshProUGUI levelText;

        public void UpdateValue(float newValue)
        {
            progressBar.value = newValue;
            levelText.text = $"{essenceType}: {newValue:F1}";
        }
    }
}