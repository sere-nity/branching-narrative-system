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
            // Only process state changes when LogRenderer isn't busy
            if (delayCounter >= 0f && !LogRenderer.IsBusy)
            {
                delayCounter -= Time.unscaledDeltaTime;
                if (delayCounter <= 0f)
                {
                    ChangeState();
                }
            }
        }
        
        private void RefreshResponseUI()
        {
            if (IsNextStateOptions)
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
            if (IsNextStateOptions)
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
            VisibilityManager.ShowElement(VisibilityManager.optionsPanel);
            VisibilityManager.HideElement(VisibilityManager.continueButton);
        }

        private void ShowContinue()
        {
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