using UnityEngine;

namespace Contagion
{
    public class ResponseUIVisibilityManager : MonoBehaviour
    {
        public RectTransform visibleArea;
        public RectTransform hiddenArea;
        public RectTransform optionsPanel;
        public RectTransform continueButton;
    
        public void ShowElement(RectTransform element)
        {
            element.SetParent(visibleArea, false);
        }
    
        public void HideElement(RectTransform element)
        {
            element.SetParent(hiddenArea, false);
        }
        
    }
}