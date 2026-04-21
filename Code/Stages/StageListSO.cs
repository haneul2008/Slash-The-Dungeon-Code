using System.Collections.Generic;
using UnityEngine;

namespace HN.Code.Stages
{
    [CreateAssetMenu(fileName = "StageList", menuName = "SO/Stage/List", order = 0)]
    public class StageListSO : ScriptableObject
    {
        public List<StageDataSO> stageDataList = new List<StageDataSO>();

        private void OnEnable()
        {
            float sum = 0;

            foreach (StageDataSO data in stageDataList)
            {
                sum += data.stagePercentage;
            }

            if (!Mathf.Approximately(sum, 100))
            {
                Debug.LogWarning("sum percentage is not 100%");
            }
        }
    }
}