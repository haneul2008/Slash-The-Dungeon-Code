using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.ETC.WarningObjects
{
    [CreateAssetMenu(fileName = "WarningObjectData", menuName = "SO/WarningObjectData", order = 0)]
    public class WarningObejctDataSO : ScriptableObject
    {
        public float duration;
        public Vector2 startSize;
        public Vector2 endSize;
        public Ease easeType;
    }
}