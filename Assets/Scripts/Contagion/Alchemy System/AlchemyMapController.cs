using UnityEngine;
using UnityEngine.UI;

namespace Contagion.Alchemy_System
{
    public class AlchemyMapController : MonoBehaviour
    {
        // Handles zooming in and panning out of the alchemy map
        [Header("References")] 
        [SerializeField] private RectTransform mapContainer; 
        [SerializeField] private Image mapBackground;
        [SerializeField] private RectTransform viewport;
        
        [Header("Zoom Settings")] 
        [SerializeField] private float zoomSpeed = 0.1f;
        [SerializeField] private float minZoom = 0.5f;
        [SerializeField] private float maxZoom = 2f;
        
        [Header("Pan Settings")]
        [SerializeField] private float panSpeed = 10f;
        [SerializeField] private bool invertPan = false;
        
        [Header("Pixel Art Settings")]
        [SerializeField] private int pixelsPerUnit = 16; // Adjust based on your art
        [SerializeField] private bool snapToPixel = false;
       
        [Header("Material Settings")]
        [SerializeField] private Material grayScaleMaterial;
       
        private Vector2 lastMousePosition;
        private float currentZoom = 1f;
        private bool isDragging = false;
        
       private void Start()
        {
            CalculateZoomLimits();
        }

        private void CalculateZoomLimits()
        {
            if (viewport != null && mapBackground != null)
            {
                float viewportWidth = viewport.rect.width;
                float viewportHeight = viewport.rect.height;
                float backgroundWidth = mapBackground.rectTransform.rect.width;
                float backgroundHeight = mapBackground.rectTransform.rect.height;

                float horizontalZoom = viewportWidth / backgroundWidth;
                float verticalZoom = viewportHeight / backgroundHeight;

                minZoom = Mathf.Max(horizontalZoom, verticalZoom);
            }
        }

        private void Update()
        { 
            HandleZoom();
            HandlePan();
            if (snapToPixel) SnapToPixelGrid();

        }

        private void HandlePan()
        {
            // if middle mouse button is pressed
            if (Input.GetMouseButtonDown(2))
            {
                isDragging = true; 
                lastMousePosition = Input.mousePosition;
            }
            
            // if middle mouse button is released
            if (Input.GetMouseButtonUp(2))
            {
                isDragging = false;
            }
            
            // if middle mouse button is held down
            if (isDragging)
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 difference = currentMousePosition - lastMousePosition;

                if (invertPan)
                    difference *= -1;

                float width = mapBackground.rectTransform.rect.width;
                float height = mapBackground.rectTransform.rect.height;
                
                float xMax = Mathf.Max(0, (width * currentZoom - viewport.rect.width) * 0.5f);
                float yMax = Mathf.Max(0, (height * currentZoom - viewport.rect.height) * 0.5f);

                Vector3 targetPosition = mapContainer.localPosition + (Vector3)difference * (panSpeed * Time.deltaTime);
                targetPosition.x = Mathf.Clamp(targetPosition.x, -xMax, xMax);
                targetPosition.y = Mathf.Clamp(targetPosition.y, -yMax, yMax);

                mapContainer.localPosition = targetPosition;
                lastMousePosition = currentMousePosition;
            }
        }

        private void HandleZoom()
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta != 0)
            {
                float newZoom = currentZoom + scrollDelta * zoomSpeed;
                newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

                if (!Mathf.Approximately(newZoom, currentZoom))
                {
                    // Convert mouse position to local space
                    Vector2 mousePos = Input.mousePosition;
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        mapContainer, mousePos, null, out localPoint);

                    // Calculate zoom center offset
                    Vector3 mouseWorldPos = mapContainer.TransformPoint(localPoint);
                    Vector3 zoomCenter = mouseWorldPos - mapContainer.position;

                    // Apply zoom from mouse position
                    mapContainer.localScale = Vector3.one * newZoom;

                    // Adjust position to keep mouse point fixed
                    Vector3 newPos = mouseWorldPos - zoomCenter * (newZoom / currentZoom);
                    Vector3 delta = newPos - mapContainer.position;
                    
                    Vector3 targetPos = mapContainer.localPosition + delta;

                    // Apply bounds
                    float width = mapBackground.rectTransform.rect.width;
                    float height = mapBackground.rectTransform.rect.height;
                    
                    float xMax = Mathf.Max(0, (width * newZoom - viewport.rect.width) * 0.5f);
                    float yMax = Mathf.Max(0, (height * newZoom - viewport.rect.height) * 0.5f);

                    targetPos.x = Mathf.Clamp(targetPos.x, -xMax, xMax);
                    targetPos.y = Mathf.Clamp(targetPos.y, -yMax, yMax);

                    mapContainer.localPosition = targetPos;
                    currentZoom = newZoom;
                }
            }
        }
        
        private void SnapToPixelGrid()
        {
            // Snap position to nearest pixel
            Vector3 position = mapContainer.localPosition;
            position.x = Mathf.Round(position.x * pixelsPerUnit) / pixelsPerUnit;
            position.y = Mathf.Round(position.y * pixelsPerUnit) / pixelsPerUnit;
            mapContainer.localPosition = position;
        }
                
        
    }
}