using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    public class SimpleResponseText : MonoBehaviour
    {
        [SerializeField] private Text responseText;  // or TMP_Text if using TextMeshPro
        [SerializeField] private RectTransform textRect;
    
        private Color _color;
        public Color color
        {
            get => responseText.color;
            set
            {
                _color = value;
                responseText.color = value;
            }
        }
    
        public string text
        {
            get => responseText.text;
            set => responseText.text = value;
        }
    
        public FontStyle fontStyle
        {
            get => responseText.fontStyle;
            set => responseText.fontStyle = value;
        }
    
        private void Awake()
        {
            if (responseText == null)
                responseText = GetComponent<Text>();
            if (textRect == null)
                textRect = GetComponent<RectTransform>();
        }
    }
}