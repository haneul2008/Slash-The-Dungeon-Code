using CSI._01.Script.Enemy;
using UnityEngine;

namespace HN.Code.Combat
{
    public class EnemyHealthSender : MonoBehaviour, IDamageable
    {
        [SerializeField] private Enemy enemy;


        public void Hurt(int damage)
        {
            enemy.Hurt(damage);
        }
    }
}