using System;
using DG.Tweening;
using HN.Code.EventSystems;
using HN.Code.Players;
using HN.Code.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace HN.Code.UI
{
    public abstract class StatUI : MonoBehaviour
    {
        [SerializeField] protected PlayerFinderSO playerFinder;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private Image fillImage;
        [SerializeField] private StatSO targetStat;

        protected StatCompo _statCompo;
        protected StatSO _stat;
        protected float _value;

        protected virtual void Start()
        {
            _statCompo = playerFinder.player.GetComponentInChildren<StatCompo>();
            uiChannel.AddListener<FadeEvent>(HandleFade);
            _stat = _statCompo.GetStat(targetStat);
            _value = _stat.BaseValue;
            _stat.OnValueChanged += HandleValueChanged;
        }

        protected virtual void OnDestroy()
        {
            uiChannel.RemoveListener<FadeEvent>(HandleFade);
        }

        protected virtual void HandleFade(FadeEvent evt)
        {
            if(evt.isActive == false)
                _stat.OnValueChanged -= HandleValueChanged;
        }

        protected virtual void SetFill()
        {
            fillImage.fillAmount = 1f;
            fillImage.DOFillAmount(0, _value + 0.1f);
        }

        private void HandleValueChanged(StatSO stat, float prev, float current)
        {
            _value = current;
        }
    }
}