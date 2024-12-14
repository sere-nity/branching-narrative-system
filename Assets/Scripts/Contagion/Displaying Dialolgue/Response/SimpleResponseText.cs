using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    public class SimpleResponseText : MonoBehaviour
    {
        [SerializeField] private TMP_Text responseText;  
    
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
        
    
        private void Awake()
        {
            if (responseText == null)
                responseText = GetComponent<TMP_Text>();
        }
    }
}