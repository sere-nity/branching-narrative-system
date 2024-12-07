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
        private bool inConversation = true;
        private bool optionResponse;
        
        public static CheckResult lastCheckResult;
        
        // Properties

        private void Start()
        {
            continueButton.SetState(ContinueState.DISABLED);
            logRenderer.ClearLog();
        }
        
        // Essential conversation flow methods
        public void OnConversationStart(Transform actor)
        {
            Debug.Log("OnConversationStart called");
            inConversation = true;
            optionResponse = false;
            // continueButton.SetState(ContinueState.DISABLED);
            logRenderer.ClearLog(); 
        }

        public void OnConversationEnd(Transform actor)
        {
            Debug.Log("OnConversationEnd called - Stack trace: " + Environment.StackTrace);
            inConversation = false;
            uiStateManager.TargetState = ResponseState.NONE;
        }
        
        // called before OnConversationLine 
        public void OnPrepareConversationLine(DialogueEntry entry)
        {
            Debug.Log($"OnPrepareConversationLine called with text: {entry.subtitleText}");
            
        
            finalEntry = null;
            if (!string.IsNullOrEmpty(entry.subtitleText))
            {
                Debug.Log("Creating final entry");

                // Create final entry
                finalEntry = new DialogueEntry(entry);
            }
        }
        
        // called just before a line is displayed 
        public void OnConversationLine(Subtitle subtitle)
        {
            Debug.Log($"OnConversationLine called with text: {subtitle.dialogueEntry.subtitleText}");
            

            if (!string.IsNullOrEmpty(subtitle.dialogueEntry.subtitleText) && inConversation)
            {
                if (optionResponse)
                {
                    optionResponse = false;
                } 
                EnableContinue(subtitle.dialogueEntry);
                if (finalEntry != null)
                {
                    logRenderer.AddToLog(finalEntry);
                    Debug.Log($"Added to log: {finalEntry.subtitleText}");
                }
            }
        }
        
        private void EnableContinue(DialogueEntry entry)
        {
            // Predict next node type
            bool isNextNodeResponse = HasResponseOptions(entry);
        
            if (isNextNodeResponse)
            {
                // Don't show continue button if next node is responses
                // Let dialogue flow directly to response options
                uiStateManager.TargetState = ResponseState.OPTIONS;
                continueButton.SetState(ContinueState.DISABLED);
                continueButton.SetInteractable(false);

            }
            else
            {
                // Show continue button for regular dialogue
                uiStateManager.TargetState = ResponseState.CONTINUE;
                continueButton.SetState(ContinueState.ENABLED);
            }
        }

        private bool HasResponseOptions(DialogueEntry entry)
        {
            var conversation = DialogueManager.MasterDatabase.GetConversation(entry.conversationID);
    
            // Check if any outgoing links lead to player responses
            foreach (var link in entry.outgoingLinks)
            {
                var targetEntry = conversation.GetDialogueEntry(link.destinationDialogueID);
                if (targetEntry.ActorID == conversation.ActorID)
                    return true;
            }
            return false;
        }


        // called when the response menu (options panel) is displayed (this is handled 
        // by the UnityUIDialogueUI class 
        public void OnConversationResponseMenu(Response[] responses)
        {
            continueButton.SetState(ContinueState.DISABLED);
            continueButton.SetInteractable(false);
            if (responses.Length != 0)
            {
                optionResponse = true;
            }
            
            // format ressponses 
            int num = 1;
            foreach (Response response in responses)
            {
                FinalResponseText finalResponseText = ChooseResponseText(response);
                // If it's visible?? when is it not visible we format the response text with num 
                if (finalResponseText.visible)
                {
                    response.formattedText.text = FormatResponse(num, finalResponseText);
                    num++;
                }
            }
          
            // set appropriate state in UI State Manager 
            uiStateManager.TargetState = ResponseState.OPTIONS;
        }  
        
        
        // TODO - figure out tags here 
        public string FormatResponse(int counter, FinalResponseText response)
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
        public FinalResponseText ChooseResponseText(Response response)
        {
            DialogueEntry destinationEntry = response.destinationEntry;
            if (CraftingCheckNode.IsCraftingCheck(destinationEntry))
            {
                // special handling of CombinedResponseText based on if its a check 
                // is in this function that gets called 
                return CraftingCheckNode.HandleResponseText(response);
            }

            string responseText = destinationEntry.subtitleText;
            return new FinalResponseText(response, responseText);
        }
        
    }
}