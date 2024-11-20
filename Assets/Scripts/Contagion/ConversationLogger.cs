using System;
using Contagion.Metric;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Contagion
{
    public class ConversationLogger : MonoBehaviour
    {
        public ResponseUIStateManager uiStateManager;
        public ContagionContinueButton continueButton; 

        public UnityUIDialogueUI unityUI;       // Base dialogue UI reference
        
        public LogRenderer logRenderer;
        private DialogueEntry finalEntry;  // Track current entry
        
        // State tracking
        private bool inConversation;
        private bool optionResponse;
        
        public static CheckResult lastCheckResult;
        
        // Properties
        public bool InConversation => inConversation;

        private void Start()
        {
            continueButton.SetState(ContinueState.DISABLED);
            logRenderer.ClearLog();
        }
        
        // Essential conversation flow methods
        public void OnConversationStart(Transform actor)
        {
            inConversation = true;
            optionResponse = false;
            continueButton.SetState(ContinueState.DISABLED);
            logRenderer.ClearLog(); 
        }

        public void OnConversationEnd(Transform actor)
        {
            inConversation = false;
            uiStateManager.TargetState = ResponseState.NONE;
        }
        
        // called before OnConversationLine 
        public void OnPrepareConversationLine(DialogueEntry entry)
        {
            finalEntry = null;
            if (!string.IsNullOrEmpty(entry.subtitleText) && inConversation)
            {
                // Create final entry
                finalEntry = new DialogueEntry(entry);
            }
        }
        
        // called just before a line is displayed 
        public void OnConversationLine(Subtitle subtitle)
        {
            if (!string.IsNullOrEmpty(subtitle.dialogueEntry.subtitleText) && inConversation)
            {   
                // if the last dialogue entry was a response 
                if (optionResponse)
                {
                    optionResponse = false;
                    /* -- WHAT HAPPENS AFTER CLICKING ON A RESPONSE --*/
                    // display continue button 
                    uiStateManager.TargetState = ResponseState.CONTINUE;
                    continueButton.SetState(ContinueState.ENABLED);
                }
                else // if we weren't in a response 
                {
                    uiStateManager.TargetState = ResponseState.CONTINUE;
                    continueButton.SetState(ContinueState.ENABLED);
                }

                // Add to log if we have a final entry
                if (finalEntry != null)
                {
                    logRenderer.AddToLog(finalEntry);
                }
            }
        }

        // called when the response menu (options panel) is displayed (this is handled 
        // by the UnityUIDialogueUI class 
        public void OnConversationResponseMenu(Response[] responses)
        {
            Debug.Log($"Response count: {responses.Length}");
            if (responses.Length != 0)
            {
                optionResponse = true;
            }

            Debug.Log("OnConversationResponseMenu");
            int num = 1;
            foreach (Response response in responses)
            {
                CombinedResponseText combinedResponseText = ChooseResponseText(response);
                // If it's visible?? when is it not visible we format the response text with num 
                if (combinedResponseText.visible)
                {
                    response.formattedText.text = FormatResponse(num, combinedResponseText);
                    num++;
                }
            }
            // set appropriate state in UI State Manager 
            uiStateManager.TargetState = ResponseState.OPTIONS;
        }  
        
        
        // TODO - figure out tags here 
        public string FormatResponse(int counter, CombinedResponseText response)
        {
            // throw new NotImplementedException();
            if (response.responseText.StartsWith("<color=") || response.responseText.StartsWith("["))
            {
                return response.responseText;
            }
            if (response.HasCheck)
            {
                if (response.enabled)
                {
                    return string.Format("[{1}] {0}", response.responseText, response.checkText);
                }
                return string.Format("[{1}] {0}", response.responseText, response.checkText);
            }
            return string.Format("{0}", response.responseText);
        }
        
        // Checks whether the next dialogue entry is a crafting check and 
        // handles it accordingly (
        public CombinedResponseText ChooseResponseText(Response response)
        {
            DialogueEntry destinationEntry = response.destinationEntry;
            if (CraftingCheckManager.IsCraftingCheck(destinationEntry))
            {
                // special handling of CombinedResponseText based on if its a check 
                // is in this function that gets called 
                return CraftingCheckManager.HandleResponseText(response);
            }

            string responseText = destinationEntry.subtitleText;
            return new CombinedResponseText(response, responseText);
        }
        
    }
}