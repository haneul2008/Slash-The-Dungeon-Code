using System;
using System.Collections;
using DG.Tweening;
using HN.Code.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace HN.Code.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private bool useStat;
        [SerializeField] private int maxHealth;
        [SerializeField] private StatSO healthStat;
        [SerializeField] private StatCompo statCompo;
        [SerializeField] private float ignoreDamageDuration;

        public bool CanHit { get; set; } = true;
        public bool IsDead => _currentHealth == 0;
        public int MaxHealth => _maxHealth;
        
        public UnityEvent OnDead;
        public UnityEvent<int> OnHit;

        protected int _maxHealth;
        protected int _currentHealth;

        protected virtual void Start()
        {
            if (useStat)
            {
                StatSO stat = statCompo.GetStat(healthStat);
                _maxHealth = (int)stat.BaseValue;
                stat.OnValueChanged += HandleHealthStatChanged;
            }
            else
            {
                _maxHealth = maxHealth;
            }
            
            _currentHealth = _maxHealth;
        }

        protected virtual void OnDestroy()
        {
            if (useStat)
            {
                StatSO stat = statCompo.GetStat(healthStat);
                stat.OnValueChanged -= HandleHealthStatChanged;
            }
        }

        public virtual void TakeDamage(int damage)
        {
            if (CanHit == false || _currentHealth == 0) return;
            
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            
            OnHit?.Invoke(_currentHealth);
            
            if(_currentHealth == 0)
                OnDead?.Invoke();
            else if (Mathf.Approximately(ignoreDamageDuration, 0) == false)
            {
                CanHit = false;
                DOVirtual.DelayedCall(ignoreDamageDuration, () => CanHit = true);
            }
        }

        protected virtual void HandleHealthStatChanged(StatSO stat, float prev, float current)
        {
            _maxHealth = (int)current;
            _currentHealth = Mathf.RoundToInt(_currentHealth * (current / prev));
        }
    }
}