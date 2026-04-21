using System;
using DG.Tweening;
using HN.Code.Combat;
using UnityEngine;

namespace HN.Code.Towers
{
    public abstract class Tower : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected int loopCnt;
        [SerializeField] protected float destroyXOffset, tweenDuration = 0.1f;
        [SerializeField] protected Health health;
        [SerializeField] protected Transform floorTrm;
        [SerializeField] protected Animator _anim;
        
        protected Player _target;
        protected SpriteRenderer _renderer;
        private float _lastAttackTime = -999f;
        private int _deadHash = Animator.StringToHash("Dead");

        public virtual void Initialize(Player target)
        {
            _target = target;
            _renderer = GetComponentInChildren<SpriteRenderer>();
            health.OnDead.AddListener(HandleDead);
        }

        protected virtual void Update()
        {
            CheckAttack();
        }

        private void CheckAttack()
        {
            if (health.IsDead) return;

            if (_lastAttackTime + attackCooldown < Time.time && CanAttack())
            {
                Attack();
                _lastAttackTime = Time.time;
            }
        }

        public void Hurt(int damage)
        {
            health.TakeDamage(damage);
        }

        private void HandleDead()
        {
            health.OnDead.RemoveListener(HandleDead);

            transform.DOMoveX(transform.position.x + destroyXOffset, tweenDuration).SetLoops(loopCnt, LoopType.Yoyo)
                .OnComplete(() => _anim.SetTrigger(_deadHash));
        }

        public abstract bool CanAttack();

        public abstract void Attack();
    }
}