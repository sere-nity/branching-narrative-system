using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion.Alchemy_System
{
    public class AlchemicalNode : MonoBehaviour
    {
        public string nodeId;
        public string displayName;
        public NodeState currentState;
        public float unlockProgress;
        public string memoryDialogueID;
    
        [SerializeField] private Image nodeImage;
        [SerializeField] internal List<AlchemicalNode> connectedNodes;
    
        public enum NodeState
        {
            Locked,        // Not yet available
            Progressing,   // Currently being unlocked through interactions
            Unlockable,    // Ready to be unlocked
            Unlocked       // Fully available
        }

        // Requirements for unlocking
        public Dictionary<string, float> copotypeRequirements; // e.g. "Scientific" : 2.0f
    
        public void UpdateProgress(string copotype, float amount)
        {
            if (currentState != NodeState.Progressing) return;
        
            if (copotypeRequirements.ContainsKey(copotype))
            {
                unlockProgress += amount;
            
                // Check if requirements met
                if (CheckRequirementsMet())
                {
                    currentState = NodeState.Unlockable;
                    OnNodeUnlockable();
                }
            }
        }

        private void OnNodeUnlockable()
        {
            throw new System.NotImplementedException();
        }

        private bool CheckRequirementsMet()
        {
            return unlockProgress >= copotypeRequirements.Values.Sum();
        }
    }

}