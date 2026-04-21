using HN.Code.Combat;
using HN.HNLib.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Towers.LightningTowers
{
    [Poolable(4)]
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private int _damage;

        public void SetUp(Vector2 pos, int damage)
        {
            transform.position = pos;
            _damage = damage;
        }

        private void Attack() => damageCaster.CastDamageOverlapBox(_damage);
        private void AnimationEnd() => poolManager.Push(this);
    }
}