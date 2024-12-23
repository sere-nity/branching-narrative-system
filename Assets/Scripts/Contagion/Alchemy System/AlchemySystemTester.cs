using UnityEngine;
using Contagion.Essence_System;

namespace Contagion.Alchemy_System
{
    public class AlchemySystemTester : MonoBehaviour
    {
        [SerializeField] private EssenceType testEssenceType = EssenceType.PURIFICATION;
        [SerializeField] private float contributionAmount = 0.25f;

        void Start()
        {
            // For testing - set initial node to Progressing using the manager
            AlchemyManager.Singleton.SetNodeStateForTesting(0, NodeState.Progressing);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AlchemyManager.Singleton.ContributeToEssence(testEssenceType, contributionAmount);
                Debug.Log($"Contributing {contributionAmount} {testEssenceType} essence");
                
                var progress = AlchemyManager.Singleton.GetEssenceProgress(testEssenceType);
                Debug.Log($"Total {testEssenceType} essence: {progress}");
            }
        }
    }
}