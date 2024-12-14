using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Contagion
{
    public class ContagionResponseButton : UnityUIResponseButton, IPointerEnterHandler, IPointerExitHandler
    {
        // UI Components
        [SerializeField] private Image backgroundImage;
        [SerializeField] private SimpleResponseText optionText;  
        
        // State tracking
        private bool isCraftingCheck;
        private DialogueEntry entry;
        
        // tooltip data tracking
        private TooltipSource tooltipSource;
        
        // Styling
        // [SerializeField] private Sprite normalBackground;
        // [SerializeField] private Sprite checkBackground;
        [SerializeField] private Color regularColor = Color.white;
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeField] private Color backgroundHighlightColor = Color.gray;

        protected new void Awake()
        {   
            base.Awake();
            // Only get components if not assigned in inspector
            if (backgroundImage == null)
                backgroundImage = GetComponent<Image>();
            tooltipSource = GetComponent<TooltipSource>();
        }
        
        protected void Start()
        {
            // base.Start();
            if (response != null)
            {
                entry = response.destinationEntry;
                GetData();
                ShowNormal();
            }
        }

        private void GetData()
        {
            if (entry == null)
            {
                Debug.LogWarning("ContagionResponseButton: DialogueEntry is null");
                return;
            }
            if (CraftingCheckNode.IsCraftingCheck(entry))
            {
                TooltipData tooltipData = new TooltipData();
                tooltipData.checkResult = CraftingCheckNode.GetCheckResult(entry);
                tooltipSource.tooltipType = TooltipType.PRE_CRAFTING_CHECK;
                tooltipSource.tooltipData = tooltipData;
                isCraftingCheck = true;
                StyleAsCraftingCheck();
            }
            else
            {
                StyleAsNormalResponse();
            }
        }

        private void StyleAsCraftingCheck()
        {
            regularColor = Color.magenta;  // Replace with your color config
            highlightColor = Color.yellow;  // Replace with your color config
            backgroundColor = Color.gray;  // Replace with your color config
            backgroundHighlightColor = Color.black;  // Replace with your color config
            
            // backgroundImage.sprite = checkBackground;
        }
        
        /// <summary>
        /// Note this is only how the button gets styled when the mouse hovers over it
        /// </summary>
        private void StyleAsNormalResponse()
        {
            // regularColor = Color.black;
            // highlightColor = Color.yellow;
            // backgroundColor = Color.white;
            // backgroundHighlightColor = Color.gray;
            
            // backgroundImage.sprite = normalBackground;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetData();
            ShowHighlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ShowNormal();
        }

        private void ShowHighlight()
        {
            if (response.enabled)
            {
                optionText.text = label.text; 
                optionText.color = highlightColor;
            }
            backgroundImage.color = backgroundHighlightColor;
        }

        private void ShowNormal()
        {
            optionText.text = label.text; 
            optionText.color = regularColor;
            backgroundImage.color = backgroundColor;
        }

        public void RegisterClick()
        {
            // TODO - why is this required?
            if (tooltipSource != null)
            {
                tooltipSource.HideTooltip();
            }
            if (isCraftingCheck)
            {
                // Calculate and store result
                ConversationLogger.lastCheckResult = 
                    CraftingCheckNode.CheckSuccess(entry);
            }
            base.OnClick();
        }
    }
}