using System.Collections;
using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.Effect
{
    [Poolable(5)]
    public class EffectPlayer : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private ParticleSystem particle;

        private WaitForSeconds _seconds;

        private void Awake()
        {
            _seconds = new WaitForSeconds(particle.main.duration);
        }

        public void SetUp(Vector2 position)
        {
            transform.position = position;
            particle.Play();
            StartCoroutine(PushCoroutine());
        }

        private IEnumerator PushCoroutine()
        {
            yield return _seconds;
            poolManager.Push(this);
        }
    }
}