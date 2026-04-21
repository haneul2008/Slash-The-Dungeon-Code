using HN.HNLib.ObjectPool;
using UnityEngine;
using UnityEngine.Events;

namespace HN.Code.Towers.IceTower
{
    public class IceTower : Tower
    {
        public UnityEvent OnFireBullet;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private Transform fireTrm;
        [SerializeField] private float detectRadius;
        [SerializeField] private int bulletCnt;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float angleAdder;
        [SerializeField] private int damage;

        private int _playerLayer;
        private float _totalAngleAdder;
        private readonly int _attackHash = Animator.StringToHash("Attack");

        public override void Initialize(Player target)
        {
            base.Initialize(target);
            
            _playerLayer = target.gameObject.layer;
        }

        public override bool CanAttack()
        {
            return Physics2D.OverlapCircle(floorTrm.position, detectRadius, 1 << _playerLayer) is not null;
        }

        public override void Attack()
        {
            float angle = 360f / bulletCnt;

            OnFireBullet?.Invoke();
            _anim.SetTrigger(_attackHash);
            
            for (int i = 0; i < bulletCnt; ++i)
            {
                IceBall iceBall = poolManager.Pop<IceBall>();
                iceBall.SetUp(fireTrm.position, angle * i + _totalAngleAdder, bulletSpeed, damage);
            }

            _totalAngleAdder += angleAdder;
        }
        
        private void OnDrawGizmos()
        {
            if(floorTrm == null) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(floorTrm.position, detectRadius);
        }
    }
}