using System;
using System.Collections.Generic;
using HN.Code.EventSystems;
using HN.Code.Gold;
using HN.Code.Managers;
using TMPro;
using UnityEngine;

namespace HN.Code.UI
{
    public class PlayerInGameUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private GameEventChannelSO goldChannel;
        [SerializeField] private HealthUI healthPrefab;
        [SerializeField] private Transform healthTrm;
        [SerializeField] private TextMeshProUGUI goldText;

        private readonly List<HealthUI> _healthList = new List<HealthUI>();
        private GoldManager _goldManager;

        private void Awake()
        {
            playerChannel.AddListener<PlayerHitEvent>(HandlePlayerHit);
            playerChannel.AddListener<PlayerHealthResetEvent>(HandlePlayerHealthReset);
            playerChannel.AddListener<PlayerDeadEvent>(HandlePlayerDead);
            goldChannel.AddListener<GoldChangeEvent>(HandleGoldChange);
        }

        private void Start()
        {
            _goldManager = CreateOnceManager.Instance.GetComponentInChildren<GoldManager>();
            goldText.text = _goldManager.CurrentGold.ToString();
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<PlayerHitEvent>(HandlePlayerHit);
            playerChannel.RemoveListener<PlayerHealthResetEvent>(HandlePlayerHealthReset);
            playerChannel.RemoveListener<PlayerDeadEvent>(HandlePlayerDead);
            goldChannel.RemoveListener<GoldChangeEvent>(HandleGoldChange);
        }

        private void HandlePlayerDead(PlayerDeadEvent evt)
        {
            gameObject.SetActive(false);
        }

        private void HandlePlayerHit(PlayerHitEvent evt)
        {
            foreach (HealthUI healthUI in _healthList)
                healthUI.SetActive(false);
            
            for(int i = 0; i < evt.hp; ++i)
                _healthList[i].SetActive(true, true);
        }

        private void HandlePlayerHealthReset(PlayerHealthResetEvent evt)
        {
            int healthInterval = evt.maxHp - _healthList.Count;
            bool isRemove = healthInterval < 0;

            for (int i = 0; i < Mathf.Abs(healthInterval); ++i)
            {
                if (isRemove)
                {
                    HealthUI targetUi = _healthList[^1];
                    _healthList.Remove(targetUi);
                    Destroy(targetUi.gameObject);
                }
                else
                {
                    HealthUI targetUi = Instantiate(healthPrefab, healthTrm);
                    _healthList.Add(targetUi);
                }
            }

            foreach (HealthUI ui in _healthList)
                ui.SetActive(false);
            
            for(int i = 0; i < evt.hp; ++i)
                _healthList[i].SetActive(true);
        }

        private void HandleGoldChange(GoldChangeEvent evt) => goldText.text = _goldManager.CurrentGold.ToString();
    }
}