using System;
using UnityEngine;

namespace HN.Code.Combat
{
    public class DamageCaster : MonoBehaviour
    {
        [SerializeField] private ContactFilter2D contactFilter;
        public int detectCnt;
        public float radius;
        public Vector2 boxSize;

        private Collider2D[] _res;

        private void Awake()
        {
            _res = new Collider2D[detectCnt];
        }

        public bool CastDamageOverlapCircle(int damage)
        {
            int cnt = Physics2D.OverlapCircle(transform.position, radius, contactFilter, _res);

            for (int i = 0; i < cnt; ++i)
            {
                if (_res[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.Hurt(damage);
                }
            }

            return cnt > 0;
        }

        public bool CastDamageOverlapBox(int damage)
        {
            int cnt = Physics2D.OverlapBox(transform.position, boxSize, 0, contactFilter, _res);
            
            for (int i = 0; i < cnt; ++i)
            {
                if (_res[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.Hurt(damage);
                }
            }

            return cnt > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}