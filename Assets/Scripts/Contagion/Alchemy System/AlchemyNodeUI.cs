using System.Collections.Generic;
using System.Linq;
using Contagion.Inventory;
using Contagion.Essence_System;
using UnityEngine;
using UnityEngine.UI;

namespace Contagion.Alchemy_System
{
    public class AlchemyNodeUI : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        public ItemData ItemData => itemData;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Dictionary<EssenceType, Image> essenceProgressCircles; // Change key type to EssenceType
        [SerializeField] private Button nodeButton;
    
        private Material grayScaleMaterial;
        private NodeState currentState;
        public NodeState CurrentState => currentState;
    
        // Events for node state changes
        public event System.Action<AlchemyNodeUI> OnNodeUnlocked;
        public event System.Action<AlchemyNodeUI> OnNodeUnlockable;
    
    

        private void Awake()
        {
            grayScaleMaterial = new Material(Shader.Find("Custom/SilhouetteSprite"));
            
            // Initialize progress circles for each essence requirement
            essenceProgressCircles = new Dictionary<EssenceType, Image>();
            
            if (itemData != null && itemData.essenceRequirements != null)  // Add null check
            {
                foreach (var requirement in itemData.essenceRequirements)
                {
                    // Look for child GameObject with name matching the essence type
                    Transform circleTransform = transform.Find($"{requirement.essenceType}Circle");
                    if (circleTransform != null)
                    {
                        var progressCircle = circleTransform.GetComponent<Image>();
                        if (progressCircle != null)
                        {
                            essenceProgressCircles[requirement.essenceType] = progressCircle;
                            progressCircle.fillAmount = 0f;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"ItemData or essenceRequirements not set for {gameObject.name}");
            }

            SetNodeState(NodeState.Locked);
            nodeButton.onClick.AddListener(HandleNodeClick);
        }

        public void SetNodeState(NodeState state)
        {
            var previousState = currentState;
            currentState = state;
        
            Debug.Log($"Node {name} state changed from {previousState} to {state}");

            // Update visuals based on state
            switch (state)
            {
                case NodeState.Locked:
                    itemIcon.material = grayScaleMaterial;
                    foreach (var progressCircle in essenceProgressCircles.Values)
                    {
                        progressCircle.fillAmount = 0f;
                    }
                    nodeButton.interactable = false;
                    break;
                
                case NodeState.Progressing:
                    itemIcon.material = grayScaleMaterial;
                    // Don't set fill amount here - UpdateProgress will handle it
                    nodeButton.interactable = false;
                    break;
                
                case NodeState.Unlockable:
                    itemIcon.material = null;
                    foreach (var progressCircle in essenceProgressCircles.Values)
                    {
                        progressCircle.fillAmount = 1f;
                    }
                    nodeButton.interactable = true;
                    OnNodeUnlockable?.Invoke(this);
                    break;
                
                case NodeState.Unlocked:
                    foreach (var progressCircle in essenceProgressCircles.Values)
                    {
                        progressCircle.fillAmount = 1f;
                    }
                    nodeButton.interactable = true;
                    if (previousState == NodeState.Unlockable)
                    {
                        OnNodeUnlocked?.Invoke(this);
                    }
                    break;
            }
        }

        public void UpdateEssenceProgress(EssenceType essenceType, float currentAmount, float requiredAmount)
        {
            if (currentState != NodeState.Progressing) return;
            
            if (essenceProgressCircles.TryGetValue(essenceType, out Image progressCircle))
            {
                float progress = Mathf.Clamp01(currentAmount / requiredAmount);
                progressCircle.fillAmount = progress;
                
                if (CheckAllRequirementsMet())
                {
                    SetNodeState(NodeState.Unlockable);
                }
            }
        }

        private bool CheckAllRequirementsMet()
        {
            foreach (var requirement in itemData.essenceRequirements)
            {
                if (essenceProgressCircles.TryGetValue(requirement.essenceType, out Image progressCircle))
                {
                    if (progressCircle.fillAmount < 1f) return false;
                }
            }
            Debug.Log("All requirements Met! Unlocking Node now....");
            return true;
        }

        public void HandleNodeClick()
        {
            Debug.Log("CLICKED!");
            // TODO - should there be a separate button linked to unlocking
            // vs opening the crafting interface? 
            if (currentState == NodeState.Unlockable)
            {
                SetNodeState(NodeState.Unlocked);
            }
            else if (currentState == NodeState.Unlocked)
            {
                // Open crafting interface
                AlchemyManager.Singleton.OpenCraftingInterface(ItemData);
                
            }
        }
    }

}