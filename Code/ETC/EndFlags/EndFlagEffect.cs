using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.ETC.EndFlags
{
    [Poolable(6)]
    public class EndFlagEffect : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;

        private void OnAnimationEnd() => poolManager.Push(this);
    }
}