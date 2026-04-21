using System;
using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class PoolManagerMono : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;

        private void Awake()
        {
            poolManager.Initialize(transform);
        }
    }
}