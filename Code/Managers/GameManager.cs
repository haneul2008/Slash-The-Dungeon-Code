using System;
using HN.Code.EventSystems;
using UnityEngine;

namespace HN.Code.Managers
{
    public class GameManager : MonoBehaviour
    {
        public float PlayTime => Mathf.RoundToInt(Time.time - _gameStartTime);
        public int ClearedStageCnt { get; private set; }

        [SerializeField] private GameEventChannelSO stageChannel;
        [SerializeField] private GameEventChannelSO gameChannel;
        
        private float _gameStartTime;

        private void Awake()
        {
            stageChannel.AddListener<StageChangeEvent>(HandleStageChange);
        }

        private void OnDestroy()
        {
            stageChannel.RemoveListener<StageChangeEvent>(HandleStageChange);
        }

        private void HandleStageChange(StageChangeEvent evt)
        {
            ClearedStageCnt = evt.depth;
        }

        public void StartGame()
        {
            _gameStartTime = Time.time;
            ClearedStageCnt = 0;
            stageChannel.RaiseEvent(StageEvents.StageInitEvent);
            gameChannel.RaiseEvent(GameEvents.GameStartEvent);
        }
    }
}