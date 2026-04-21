using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.ETC.Scene
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "SO/SceneData", order = 0)]
    public class SceneDataSO : ScriptableObject
    {
        public string sceneName;
        public bool isSavePlayerData;
    }
}