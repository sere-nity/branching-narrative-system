using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Contagion
{
    [RequireComponent(typeof(ScrollRect))]
    public class AutoScroller : SingletonMonoBehaviour<AutoScroller>
    {
        [FormerlySerializedAs("targetMarker")]
        [Header("Markers")]
        [SerializeField] private RectTransform staticMarker;
        [SerializeField] private RectTransform staticTopMarker;
        [SerializeField] private RectTransform topMarker;
        [SerializeField] private RectTransform botMarker;
        [SerializeField] private RectTransform movingMarker;
        [SerializeField] private RectTransform staticBotMarker;
        
        [Tooltip("THIS MUST BE THE RECT TRANSFORM OF THE LOG STACK")]
        [SerializeField] private RectTransform targetRect; // this is the rect transform of the log stack

        [Header("This is a temp fix")] public bool tempFix; 
        
        private ScrollRect scrollRect;
        // private Scrollbar scrollBar;
        private bool logEmpty;
        private bool waitingForContentToStabilize;
        private int scrollCounter;
        private Scrollbar scrollBar; 
        private Vector3 lastWorldPosition; 
        
        public float smoothSpeed = 50f;

        protected override void Awake()
        {
            base.Awake();
            scrollRect = GetComponent<ScrollRect>();
            // targetRect = scrollRect.content;
            scrollBar = scrollRect.verticalScrollbar;
            // initially when this game object i.e. the scrolling panel is set to active, 
            // we enter a conversation so the log stack must be empty 
            logEmpty = true;
        }

        private void Update()
        {
            if (waitingForContentToStabilize)
            {
                if (lastWorldPosition != Vector3.zero && lastWorldPosition == targetRect.position)
                {
                    waitingForContentToStabilize = false;
                    lastWorldPosition = Vector3.zero;
                    PrepareAndTriggerScrolling();
                }
                else
                {
                    SetContentTransform(staticMarker, targetRect, movingMarker);
                    lastWorldPosition = targetRect.position;
                }
            }
            if (scrollCounter > 0)
            {
                ScrollTowardsTargetHeight();
            }
        }
        
        /// <summary>
        /// Should be called whenever we add stuff to the log stack
        /// </summary>
        public void LogWasUpdated()
        {
            Debug.Log("Log was updated");;
            // move the position of the log stack to the target marker at the beginning
            if (logEmpty)
            {
                SetInitialPosition();
                logEmpty = false; 
            }
            
            // do the setup needed to begin auto scrolling and trigger the scrolling
            PrepareAndTriggerScrolling();
        }

        public void OnValueChanged()
        {
            if (GetYWorldPosition(topMarker) < GetYWorldPosition(staticMarker))
            {
                SetContentTransform(staticMarker, targetRect, topMarker);
            } else if (GetYWorldPosition(movingMarker) > GetYWorldPosition(staticTopMarker))
            {
                SetContentTransform(staticTopMarker, targetRect, movingMarker);
            }
        }
        
        private void PrepareAndTriggerScrolling()
        {
            // make sure the viewport has updated its layout before we begin scrolling
            // note the targetRect.parent is the viewport
            LayoutRebuilder.ForceRebuildLayoutImmediate(targetRect.parent.transform as RectTransform);
            
            
            // update the scrolling counter - this is picked up by the Update function which will begin scrolling
            if (scrollCounter > 0)
            {
                Debug.Log("INCREMENTING SCROLL COUNTER");
                scrollCounter++;
            }
            else
            {
                scrollCounter = 1; 
            }
            
            // we want to disable the scrollbar before auto scrolling begins
            scrollBar.enabled = false;
        }
        private void ScrollTowardsTargetHeight()
        {
            Debug.Log("SCROLLING TOWARDS TARGET HEIGHT");
            // get marker positions
            float fixedHeight = GetYWorldPosition(staticMarker);
            float bottomOfStack = GetYWorldPosition(movingMarker);
            float distanceBelow = fixedHeight - bottomOfStack;
            bool isBelow = distanceBelow > 0;
            
            // get bottom boundary positions 
            float bottom = GetYWorldPosition(staticBotMarker);
            float botOfButtons = GetYWorldPosition(botMarker);
            float distanceBelowBottom = bottom - botOfButtons;
            bool isOverflowing = distanceBelowBottom > 0;
            
            // calculate scroll speed
            float screenScaleFactor = Screen.height / 1080f;
            float baseScrollSpeed = 500f * screenScaleFactor;
            // remember d = vt but this is only one unit of distance... 
            float scrollDelta = baseScrollSpeed * Time.unscaledDeltaTime;
            
            Debug.Log($"Fixed Height: {fixedHeight}, Bottom of Stack: {bottomOfStack}");
            Debug.Log($"Is Below: {isBelow}, Is Overflowing: {isOverflowing}");
            
            // if we have finished scrolling
            if (!isBelow && !isOverflowing)
            {
                // decrements so we can signal to the update function to stop calling this function
                scrollCounter--;            
                scrollBar.enabled = true;
                Debug.Log("RETURNING");
                // could add like extra stuff here like updating the scrollbar
                return; 
            }
            

            // if the log entry or response buttons are reaaallly long then we must scroll the 
            // distanceBelowBottom amount 
            if (isBelow && isOverflowing)
            {
                // choose larger distance to ensure we scroll enough
                float scrollDistance = distanceBelow > distanceBelowBottom ? distanceBelow : distanceBelowBottom;
                scrollDelta = Mathf.Clamp(scrollDistance, 0 - scrollDelta, scrollDelta);
            }
            else
            {
                // if isOverflowing is true, isBelow is false - we use the distance below the bottom of the stack
                // if isOverflowing is false, isBelow is true - we use the distance below the target height
                // if isOverflowing is false, isBelow is false - we use the distance below the target height
                // THE ABOVE CAN BE CAPTURED IN A BOOLEAN EXPRESSION
                float scrollDistance = (!isOverflowing || isBelow) ? distanceBelow : distanceBelowBottom;
                scrollDelta = Mathf.Clamp(scrollDistance, 0 - scrollDelta, scrollDelta);
            }
          
            
            // Ensure minimum scroll speed
            scrollDelta = Mathf.Clamp(scrollDelta, 1f, baseScrollSpeed / 10f + 2f);
            
            Debug.Log($"Scroll Delta: {scrollDelta}");
            // Apply scroll position
            Vector3 position = targetRect.position;
            position.y += scrollDelta;
            // position.y = topMarker.position.y + scrollDelta;
            targetRect.position = position;
        }

  

        private void SetInitialPosition()
        {
            // TODO - not sure if this is necessary - but might be necessary if we want to 
            // reset the conversation from elsewhere instead of at the beginning of this class in Awake()
            // logEmpty = true;
            Debug.Log("SETTING INITIAL POSITION");
            // move the position of the log stack to the static marker at the beginning
            SetContentTransform(staticMarker, targetRect, movingMarker);

            waitingForContentToStabilize = true; 
        }

        private float GetYWorldPosition(RectTransform rect)
        {
            return rect.transform.position.y;
        }
        
        /// <summary>
        /// Should be called at the beginning to move the position of the log stack to the target marker
        /// Moves the position of current to next with respect to the reference 
        /// </summary>
        private void SetContentTransform(RectTransform next, RectTransform current, RectTransform reference)
        {
     
            if (tempFix)
            {
                Vector3 currentPos = current.position;
                float offset = topMarker.transform.position.y - reference.transform.position.y;
                Vector3 targetPos = new Vector3(currentPos.x, next.position.y + offset, currentPos.z);
            
                current.position = Vector3.Lerp(currentPos, targetPos, Time.unscaledDeltaTime * smoothSpeed);
            }
            else
            {
                Vector3 position = current.position;
            
                // this is the distance between the top and bottom of the log stack - this would be zero initially
                float offset = topMarker.transform.position.y - reference.transform.position.y;
                position.y = next.transform.position.y + offset;
                current.position = position;
            }
    

        }
        
        
        
        
    }
}