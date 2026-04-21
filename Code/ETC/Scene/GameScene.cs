using System;
using HN.Code.EventSystems;
using UnityEngine;

namespace HN.Code.ETC.Scene
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO stageChannel;

        private void Start()
        {
            stageChannel.RaiseEvent(StageEvents.StageSpawnEvent);
        }
    }
}