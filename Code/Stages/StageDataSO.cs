using HN.Code.ETC.Scene;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Stages
{
    [CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage/StageData", order = 0)]
    public class StageDataSO : ScriptableObject
    {
        public float stagePercentage;
        public Sprite mapSprite;
        public SceneDataSO sceneData;
    }
}