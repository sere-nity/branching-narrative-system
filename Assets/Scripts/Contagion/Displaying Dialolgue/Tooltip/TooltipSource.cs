using UnityEngine;
using UnityEngine.EventSystems;

namespace Contagion
{
    public class TooltipSource : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Tooltip Type")]
        public TooltipType tooltipType;
        
        [Header("Tooltip Data")]
        public TooltipData tooltipData; 
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Debug.Log($"OnPointerEnter called on {gameObject.name}"); // NEW
            ShowTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
        }

        private void ShowTooltip()
        {
            IToolTip tooltipFromType = GetTooltipFromType(tooltipType);
            if (tooltipFromType != null)
            {
                tooltipFromType.Show(this);
            }
        }

        public void HideTooltip()
        {
            IToolTip tooltipFromType = GetTooltipFromType(tooltipType);
            if (tooltipFromType != null)
            {
                tooltipFromType.Hide(this);
            }
        }

        private IToolTip GetTooltipFromType(TooltipType tooltipType)
        {
            switch (tooltipType)
            {
                case TooltipType.PRE_CRAFTING_CHECK:
                    return SingletonMonoBehaviour<PreCraftingCheckTooltip>.Singleton;
                case TooltipType.POST_CRAFTING_CHECK:
                    if (tooltipData != null && tooltipData.checkResult != null)
                    {
                        return SingletonMonoBehaviour<PostCraftingCheckTooltip>.Singleton;
                    }
                    else
                    {
                        Debug.Log("TooltipData or CheckResult is null");
                        return null;
                    }
                case TooltipType.GENERIC:
                    return null; // TODO 
                default:
                    return null; 
            }
        }


    }
    
}