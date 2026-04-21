using System;
using HN.HNLib.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Towers.IceTower
{
    [Poolable(8)]
    public class IceBall : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private Animator anim;

        private Rigidbody2D _rigid;
        private Collider2D _collider;
        private float _speed;
        private int _damage;
        private bool _isCollide;
        private readonly int _deadHash = Animator.StringToHash("Dead");

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public void SetUp(Vector2 pos, float angle, float speed, int damage)
        {
            transform.position = pos;
            _speed = speed;
            _damage = damage;

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        private void Update()
        {
            if (_isCollide) return;
            _rigid.linearVelocity = transform.right * _speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                player.Hurt(_damage);
            }
            
            _isCollide = true;
            anim.SetBool(_deadHash, true);
            _rigid.linearVelocity = Vector2.zero;
            _collider.enabled = false;
        }

        private void AnimationEnd()
        {
            anim.SetBool(_deadHash, false);
            poolManager.Push(this);
        }

        [ResetItem]
        private void ResetItem()
        {
            _collider.enabled = true;
            _isCollide = false;
        }
    }
}