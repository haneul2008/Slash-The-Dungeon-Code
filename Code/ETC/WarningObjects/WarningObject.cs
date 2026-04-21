using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.ETC.WarningObjects
{
    [Poolable(5)]
    public class WarningObject : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private SpriteRenderer baseRenderer, centerRenderer;

        public void SetUp(Vector2 position, WarningObejctDataSO data, Action<Vector2> onComplete = null)
        {
            transform.position = position;
            baseRenderer.size = data.endSize;
            centerRenderer.size = data.startSize;
            DOTween.To(() => centerRenderer.size, value => centerRenderer.size = value, data.endSize, data.duration).SetEase(data.easeType)
                .OnComplete(() =>
                {
                    onComplete?.Invoke(position);
                    poolManager.Push(this);
                });
        }

        [ResetItem]
        private void ResetItem()
        {
            centerRenderer.size = Vector2.zero;
        }
    }
}