namespace Contagion
{
    public interface IToolTip
    {
        void Show(TooltipSource source);
        void Hide(TooltipSource source);
        void SetTooltipContent(TooltipSource source);
    }
}