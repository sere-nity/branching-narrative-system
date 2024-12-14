using UnityEngine;
using UnityEngine.UI;

namespace Contagion
{
    public class ContagionContinueButton : MonoBehaviour
    {
        private Button button;
        private ContinueState state = ContinueState.DISABLED;
        private ConversationLogger conversationLogger;


        private void Awake()
        {
            button = GetComponent<Button>();
            conversationLogger = FindObjectOfType<ConversationLogger>();

        }
        
        public void OnContinueClicked()
        {
            if (state != ContinueState.DISABLED)
            {
                // Tell UnityUIDialogueUI to continue
                conversationLogger.unityUI.OnContinue();
            }
        }

        public void SetInteractable(bool interactable)
        {
            if (button != null) button.interactable = interactable;
        }
        
        public void SetState(ContinueState newState)
        {
            state = newState;
            UpdateButtonState();
        }
        
        private void UpdateButtonState()
        {
            // Update button appearance based on state
            switch (state)
            {
                case ContinueState.DISABLED:
                    SetInteractable(false);
                    break;
                case ContinueState.ENABLED:
                    SetInteractable(true);
                    break;
                case ContinueState.END_CONVERSATION:
                    SetInteractable(true);
                    break;
            }
        }
    }

}