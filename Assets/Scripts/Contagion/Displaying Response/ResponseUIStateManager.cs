using System;
using UnityEngine;

namespace Contagion
{
    public class ResponseUIStateManager : MonoBehaviour
    {
        public ResponseUIVisibilityManager VisibilityManager;
        
        private ResponseState targetState;

        private ResponseState currentState;

        private float delayCounter;

        public float delay; 
        
        private bool IsNextStateOptions
        {
            get
            {
                // if we're not changing to OPTIONS state 
                if (targetState != ResponseState.OPTIONS)
                {
                    // check to see if we're changing state normally 
                    return targetState != currentState;
                }
                // if we are changing to OPTIONS always true 
                return true;
            }
        }
    

        // since the update function of this class is continuously 
        // listening for the state and responding, we 
        // kinda want a delay before we display the options 
        // hence we introduce a delay when setting the response 
        // state. This is mainly for smooth transitioning 
        // between displaying the responses and displaying 
        // the continue button 
        public ResponseState TargetState
        {
            set
            {
                targetState = value;
                RefreshResponseUI(); 
            }
        
        }
        
        private void Start()
        {
            currentState = ResponseState.NONE;
        }

        private void Update()
        {
            // busy wait UI Response State
            if (delayCounter >= 0f)
            {
                delayCounter -= Time.unscaledDeltaTime;
                ChangeState(); 
            }
        }
        
        private void RefreshResponseUI()
        {
            if (targetState != currentState)
            {
                // hide everything 
                ResetDisplay();
                // reset the delay counter 
                delayCounter = delay;
            }
            else
            {
                delayCounter = -1; 
            }
            
        }

        private void ChangeState()
        {
            if (delayCounter <= 0f)
            {
                switch (targetState)
                {
                    case ResponseState.NONE:
                        HideAll();
                        break;
                    case ResponseState.CONTINUE:
                        ShowContinue();
                        break;
                    case ResponseState.OPTIONS:
                        ShowOptions();
                        break;
                }
                currentState = targetState;
            }
        }

        private void ResetDisplay()
        {
            VisibilityManager.HideElement(VisibilityManager.optionsPanel);
            VisibilityManager.HideElement(VisibilityManager.continueButton);
            currentState = ResponseState.NONE;
        }

        private void ShowOptions()
        {
            Debug.Log("ShowOptions");
            VisibilityManager.ShowElement(VisibilityManager.optionsPanel);
            VisibilityManager.HideElement(VisibilityManager.continueButton);
        }

        private void ShowContinue()
        {
            Debug.Log("Showing Continue Button");
            VisibilityManager.HideElement(VisibilityManager.optionsPanel);
            VisibilityManager.ShowElement(VisibilityManager.continueButton);
        }

        private void HideAll()
        {
            VisibilityManager.HideElement(VisibilityManager.optionsPanel);
            VisibilityManager.HideElement(VisibilityManager.continueButton);
        }

       

 
    }
}