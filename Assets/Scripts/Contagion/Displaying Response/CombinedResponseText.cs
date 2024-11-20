using PixelCrushers.DialogueSystem;

namespace Contagion
{
    public class CombinedResponseText
    {
        public Response response;           // Current response button
        public string responseText;         // Next dialogue entry's text
        public string checkText;           // Check-related info if any
        public bool visible = true;
        private bool _enabled = true;

        public bool HasCheck => checkText != null; 
        
        public bool enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (response != null)
                {
                    response.enabled = value;
                }
            }
        }

        public CombinedResponseText(Response response, string responseText)
        {
            this.response = response;
            this.responseText = responseText;
        }
        
        
    }
}