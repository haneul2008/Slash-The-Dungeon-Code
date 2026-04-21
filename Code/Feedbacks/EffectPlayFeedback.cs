using System;
using HN.Code.Effect;
using HN.HNLib.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Feedbacks
{
    public class EffectPlayFeedback : Feedback
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private string effectName;
        [SerializeField] private Transform spawnTrm;
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(effectName))
            {
                Debug.LogWarning($"effect name is null : {gameObject.name}");
            }
        }

        public override void PlayFeedback()
        {
            EffectPlayer effectPlayer = poolManager.Pop<EffectPlayer>(effectName);
            effectPlayer?.SetUp(spawnTrm.position);
        }

        public override void StopFeedback()
        {
            
        }
    }
}