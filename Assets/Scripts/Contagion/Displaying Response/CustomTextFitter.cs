using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    /// <summary>
    /// This script is used to make sure the rect transform holding the log entry text
    /// dynamically resizes to contain the text.
    /// </summary>
    public class CustomTextFitter : MonoBehaviour
    {
        public Text textComponent;
        public LayoutElement layoutElement;
        public float maxWidth = 0f;
        // Idk why this is 32f but it works!! 
        public float pixelsPerUnit = 32f;

        void Update()
        {
            CalculateLayoutSize();
        }

        void CalculateLayoutSize()
        {
            var generator = new TextGenerator();
            var settings = textComponent.GetGenerationSettings(new Vector2(maxWidth, 0));
            
            // Get height in pixels and convert to Unity units
            float heightInPixels = generator.GetPreferredHeight(textComponent.text, settings);
            float heightInUnits = heightInPixels / pixelsPerUnit;
            layoutElement.preferredHeight = heightInUnits;

        }
    }  
}
    
