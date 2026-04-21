using System.Collections;
using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.ETC.EndFlags
{
    public class EndFlag : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private Vector3 endPosOffset = new Vector3(0, 0.25f, 0);
        [SerializeField] private float effectSpawnDelay;

        private WaitForSeconds _seconds;
        
        private void Awake()
        {
            _seconds = new WaitForSeconds(effectSpawnDelay);
            StartCoroutine(EffectSpawnCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(EffectSpawnCoroutine());
        }

        private IEnumerator EffectSpawnCoroutine()
        {
            while (true)
            {
                EndFlagEffect effect = poolManager.Pop<EndFlagEffect>();
                effect.transform.SetParent(transform);
                yield return _seconds;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.MoveToEndFlag(transform.position + endPosOffset);
            }
        }
    }
}