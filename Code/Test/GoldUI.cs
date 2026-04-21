using System;
using HN.Code.EventSystems;
using HN.Code.Gold;
using TMPro;
using UnityEngine;

namespace HN.Code.Test
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO goldChannel;
        [SerializeField] private TextMeshProUGUI goldText;
        
        private GoldManager goldManager;

        private void Awake()
        {
            GoldEvents.GoldChangeEvent.onChangeGold += HandleGoldChange;

            goldManager = FindAnyObjectByType<GoldManager>();
            goldText.text = goldManager.CurrentGold.ToString();
        }

        private void OnDestroy()
        {
            GoldEvents.GoldChangeEvent.onChangeGold -= HandleGoldChange;
        }

        private void HandleGoldChange(bool isSuccess)
        {
            if (isSuccess)
                goldText.text = goldManager.CurrentGold.ToString();
        }
    }
}