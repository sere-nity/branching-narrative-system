using System;
using System.Collections.Generic;
using Contagion;
using Contagion.Essence_System;
using Contagion.Inventory;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Contagion.Alchemy_System
{
    public class AlchemyManager : SingletonMonoBehaviour<AlchemyManager>
    {
    
        [SerializeField] private List<AlchemyNodeUI> allNodes;
        [SerializeField] private GameObject craftingInterface; // Reference to the crafting UI panel
        
        private Dictionary<string, float> essenceProgress = new Dictionary<string, float>();
    
        public event Action<AlchemyNodeUI> OnNodeUnlocked;
        public event Action<AlchemyNodeUI> OnNodeUnlockable;
        public event Action<EssenceType, float> OnEssenceProgressed;

        protected override void Awake()
        {
            base.Awake();
            // Subscribe to node events
            foreach (AlchemyNodeUI node in allNodes)
            {
                node.OnNodeUnlocked += HandleNodeUnlocked;
                node.OnNodeUnlockable += HandleNodeUnlockable;
            }
        }

        public void ContributeToEssence(EssenceType essenceType, float amount)
        {
            EssenceManager.Singleton.AddEssence(essenceType, amount);
            OnEssenceProgressed?.Invoke(essenceType, amount);

            // Update nodes using the global essence level
            foreach (AlchemyNodeUI node in allNodes)
            {
                if (node.CurrentState == NodeState.Progressing)
                {
                    var requirement = node.ItemData.essenceRequirements.Find(r => r.essenceType == essenceType);
                    if (requirement != null)
                    {
                        float currentAmount = EssenceManager.Singleton.GetEssenceLevel(essenceType);
                        node.UpdateEssenceProgress(essenceType, currentAmount, requirement.amount);
                    }
                }
            }
        }

    
        private void HandleNodeUnlocked(AlchemyNodeUI nodeUI)
        {
            OnNodeUnlocked?.Invoke(nodeUI);
            
            // Trigger memory/dialogue sequence
            if (!string.IsNullOrEmpty(nodeUI.ItemData.memoryConversationName))
            {
                DialogueManager.StartConversation(nodeUI.ItemData.memoryConversationName);
            }
            
            // Set connected nodes to Progressing if they're Locked
            // foreach (var connectedNode in GetConnectedNodes(node))
            // {
            //     if (connectedNode.CurrentState == AlchemyNode.NodeState.Locked)
            //     {
            //         connectedNode.SetNodeState(AlchemyNode.NodeState.Progressing);
            //     }
            // }
        }

        private void HandleNodeUnlockable(AlchemyNodeUI nodeUI)
        {
            OnNodeUnlockable?.Invoke(nodeUI);
            // Add any additional effects when a node becomes unlockable
        }

        public void OpenCraftingInterface(ItemData itemData)
        {
            if (craftingInterface != null)
            {
                craftingInterface.SetActive(true);
                // TODO: Set up crafting interface with item data
            }
        }

        private List<AlchemyNodeUI> GetConnectedNodes(AlchemyNodeUI nodeUI)
        {
            // TODO: Implement node connection logic
            // This could be based on a serialized list of connections,
            // or determined programmatically based on game progression rules
            return new List<AlchemyNodeUI>();
        }
        
        /* ---- TESTING FUNCTIONS ---- */
        public float GetEssenceProgress(EssenceType essenceType)
        {
            return EssenceManager.Singleton.GetEssenceLevel(essenceType);
        }
        
        public void SetNodeStateForTesting(int nodeIndex, NodeState state)
        {
            if (nodeIndex >= 0 && nodeIndex < allNodes.Count)
            {
                allNodes[nodeIndex].SetNodeState(state);
            }
        }
        
    }
}