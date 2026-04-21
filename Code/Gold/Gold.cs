using System;
using System.Collections;
using HN.Code.EventSystems;
using HN.Code.Items;
using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.Gold
{
    [Poolable(5, "GOLD")]
    public class Gold : Collectable
    {
        [SerializeField] private GameEventChannelSO goldChannel;
        [SerializeField] private PoolManagerSO poolManager;
        
        public override void Collect(Transform collector, float magneticPower)
        {
            if (_alreadyCollected || !_canCollectable) return;
            _collider.enabled = false;
            _alreadyCollected = true;

            StartCoroutine(CollectCoroutine(collector, magneticPower));
        }

        private IEnumerator CollectCoroutine(Transform collector, float magneticPower)
        {
            float distance = Vector2.Distance(transform.position, collector.position);
            float time = distance / magneticPower;
            float currentTime = 0;

            Vector3 startPosition = transform.position;

            while (currentTime <= time)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / time;
                transform.position = Vector3.Lerp(startPosition, collector.position, t*t*t);
                yield return null;
            }
        
            int amount = itemData.GetRandomAmount();
            
            goldChannel.RaiseEvent(GoldEvents.GoldChangeEvent.Initializer(amount));
            SoundManager.Instance.PlaySound("GetCoin");
            poolManager.Push(this);
        }

        [ResetItem]
        public void ResetItem()
        {
            _alreadyCollected = false;
            _canCollectable = false;
            _collider.enabled = true;
        }

    }
}