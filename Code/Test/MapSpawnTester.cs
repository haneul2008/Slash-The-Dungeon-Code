using HN.Code.EventSystems;
using HN.Code.Stages;
using UnityEngine;

namespace HN.Code.Test
{
    public class MapSpawnTester : MonoBehaviour
    {
        [SerializeField] private StageDataSO stageData;
        [SerializeField] private GameEventChannelSO stageChannel;

        [ContextMenu("SpawnMap")]
        public void SpawnMap()
        {
            StageEvents.StageSelectEvent.tempData = stageData;
            stageChannel.RaiseEvent(StageEvents.StageSelectEvent);
            stageChannel.RaiseEvent(StageEvents.StageSpawnEvent);
        }
    }
}