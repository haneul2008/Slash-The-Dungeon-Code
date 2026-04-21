using System;
using HN.Code.Combat;
using HN.Code.EventSystems;
using HN.Code.Stats;
using UnityEngine;

namespace HN.Code.Players
{
    public class PlayerHealth : Health
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        
        protected override void Start()
        {
            base.Start();
            
            OnHit.AddListener(HandleHit);
            playerChannel.RaiseEvent(PlayerEvents.PlayerHealthResetEvent.Initializer(_currentHealth, _maxHealth));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnHit.RemoveListener(HandleHit);
        }

        private void HandleHit(int hp)
        {
            playerChannel.RaiseEvent(PlayerEvents.PlayerHitEvent.Initializer(hp));
        }

        protected override void HandleHealthStatChanged(StatSO stat, float prev, float current)
        {
            base.HandleHealthStatChanged(stat, prev, current);
            
            playerChannel.RaiseEvent(PlayerEvents.PlayerHealthResetEvent.Initializer(_currentHealth, _maxHealth));
        }

        public int GetSaveData() => _currentHealth;

        public void RestoreData(int currentHealth)
        {
            _currentHealth = currentHealth;
            playerChannel.RaiseEvent(PlayerEvents.PlayerHealthResetEvent.Initializer(_currentHealth, _maxHealth));
        }
    }
}