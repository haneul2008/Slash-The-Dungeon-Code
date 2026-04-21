using System;
using HN.Code.EventSystems;
using HN.Code.Managers;
using UnityEngine;

namespace HN.Code.Gold
{
    public class GoldManager : MonoBehaviour
    {
        public int CurrentGold { get; private set; }

        [SerializeField] private GameEventChannelSO goldChannel;
        [SerializeField] private GameEventChannelSO gameChannel;

        private void Awake()
        {
            goldChannel.AddListener<GoldChangeEvent>(HandleGoldChange);
            gameChannel.AddListener<GameStartEvent>(HandleGameStart);
        }

        private void OnDestroy()
        {
            goldChannel.RemoveListener<GoldChangeEvent>(HandleGoldChange);
            gameChannel.RemoveListener<GameStartEvent>(HandleGameStart);
        }

        private void HandleGameStart(GameStartEvent evt)
        {
            CurrentGold = 0;
        }

        private void HandleGoldChange(GoldChangeEvent evt)
        {
            if (CurrentGold < -evt.gold)
            {
                evt.onChangeGold?.Invoke(false);
            }
            else
            {
                CurrentGold += evt.gold;
                evt.onChangeGold?.Invoke(true);
            }
        }
    }
}