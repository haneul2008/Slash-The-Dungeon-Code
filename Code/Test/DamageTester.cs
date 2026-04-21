using UnityEngine;

namespace HN.Code.Test
{
    public class DamageTester : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private int damage;

        [ContextMenu("Take Damage")]
        public void TakeDamage()
        {
            if(target.TryGetComponent(out IDamageable damageable))
                damageable.Hurt(damage);
        }
    }
}