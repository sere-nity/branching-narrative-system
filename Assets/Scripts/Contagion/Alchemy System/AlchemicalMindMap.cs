using System;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion.Alchemy_System
{
    public class AlchemicalMindMap : MonoBehaviour
    {
        public static AlchemicalMindMap Instance { get; private set; }
    
        [SerializeField] private List<AlchemicalNode> allNodes;
        private Dictionary<string, float> copotypeProgress = new Dictionary<string, float>();
    
        public event Action<AlchemicalNode> OnNodeUnlocked;
        public event Action<string, float> OnCopotypeProgressed;

        public void ContributeToCopotype(string copotype, float amount)
        {
            if (!copotypeProgress.ContainsKey(copotype))
                copotypeProgress[copotype] = 0;
            
            copotypeProgress[copotype] += amount;
            OnCopotypeProgressed?.Invoke(copotype, amount);
        
            // Update all progressing nodes
            foreach (var node in allNodes)
            {
                if (node.currentState == AlchemicalNode.NodeState.Progressing)
                {
                    node.UpdateProgress(copotype, amount);
                }
            }
        }

        public void UnlockNode(string nodeId)
        {
            var node = allNodes.Find(n => n.nodeId == nodeId);
            if (node != null && node.currentState == AlchemicalNode.NodeState.Unlockable)
            {
                node.currentState = AlchemicalNode.NodeState.Unlocked;
                OnNodeUnlocked?.Invoke(node);
            
                // Trigger memory/dialogue sequence using Pixel Crushers
                if (!string.IsNullOrEmpty(node.memoryDialogueID))
                {
                    DialogueManager.StartConversation(node.memoryDialogueID);
                }
            
                // Set connected nodes to Progressing if they're Locked
                foreach (var connectedNode in node.connectedNodes)
                {
                    if (connectedNode.currentState == AlchemicalNode.NodeState.Locked)
                    {
                        connectedNode.currentState = AlchemicalNode.NodeState.Progressing;
                    }
                }
            }
        }
    }
}