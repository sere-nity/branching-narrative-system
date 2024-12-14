using Contagion;
using UnityEngine;

public abstract class BaseTooltip<T> : SingletonMonoBehaviour<T>, IToolTip where T : MonoBehaviour
{
    protected bool followMouseCursor = true;
    protected RectTransform rectTransform;
    private TooltipSource _source;
    private bool positionNeedsUpdate;

    public virtual TooltipSource Source
    {
        get => _source;
        set
        {
            Debug.Log($"Source setter called. Current: {_source}, New: {value}"); // NEW
            // Only update if source actually changes
            if (_source != value)
            {
                _source = value;
                if (_source != null)
                {
                    SetTooltipContent(_source);
                    positionNeedsUpdate = true;
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    
    protected virtual void OnEnable()
    {
        if (_source != null)
        {
            SetTooltipContent(Source);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public virtual void Show(TooltipSource source)
    {
        Source = source; 
    }

    public virtual void Hide(TooltipSource source)
    {
        if (source == _source)
        {
            Source = null; 
        }
    }

    protected virtual void LateUpdate()
    {
        if (positionNeedsUpdate)
        {
            UpdatePosition();
            positionNeedsUpdate = false;
        }
    }

    protected virtual void UpdatePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos += new Vector2(10f, 10f);
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width - rectTransform.rect.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height - rectTransform.rect.height);
        rectTransform.position = mousePos;
    }

    public abstract void SetTooltipContent(TooltipSource source);
}