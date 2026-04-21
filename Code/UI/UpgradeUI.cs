using System;
using System.Text;
using DG.Tweening;
using HN.Code.EventSystems;
using HN.Code.Reference.Texts;
using HN.Code.Stats;
using HN.Code.Upgrades.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HN.Code.UI
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backGroundImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI statText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextContainerSO textContainer;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private float failTweenDuration;

        public UpgradeDataSO UpgradeData { get; private set; }
        private Tween _colorTween;
        private bool _isSuccess;
        private float _saleMultiplier = 1;

        public void SetUp(UpgradeDataSO upgradeData, bool isSale, float salePercent)
        {
            UpgradeData = upgradeData;
            iconImage.sprite = upgradeData.upgradeImage;
            nameText.text = upgradeData.upgradeName;
            
            _saleMultiplier = isSale ? salePercent : 1;
            
            int requireGold = Mathf.RoundToInt(upgradeData.requireGoldAmount * _saleMultiplier);
            string saleText = isSale ? $"(-{Mathf.RoundToInt((1 - salePercent) * 100).ToString()}%)" : "";
            goldText.text = $"{saleText} {requireGold} Gold";
            
            StringBuilder statBuilder = new StringBuilder();
            foreach (UpgradeStat stat in upgradeData.upgradeStats)
            {
                TextDataSO textData = textContainer.GetTextData(stat.targetStat.statName);
                if (textData is null)
                {
                    Debug.LogWarning($"textData is null : {stat}");
                    continue;
                }

                string line = $"+ <b><color={textData.color}>{textData.text[0]}:</color></b>{stat.value}\n";
                statBuilder.Append(line);
            }

            statText.text = statBuilder.ToString();
        }

        public void BuyUpgrade()
        {
            if(_isSuccess) return;
            bool isSale = Mathf.Approximately(_saleMultiplier, 1) == false;
            uiChannel.RaiseEvent(UIEvents.BuyUpgradeEvent.Initializer(this, isSale, _saleMultiplier));
        }

        public void PlayColorTween(bool isSuccess, Color color, Action onComplete = null)
        {
            _colorTween?.Complete();
            _colorTween = backGroundImage.DOColor(color, failTweenDuration).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => onComplete?.Invoke());
            _isSuccess = isSuccess;
        }
    }
}