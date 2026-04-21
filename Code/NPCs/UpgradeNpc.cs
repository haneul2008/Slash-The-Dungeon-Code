using System;
using System.Collections.Generic;
using System.Linq;
using HN.Code.EventSystems;
using HN.Code.Upgrades.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace HN.Code.NPCs
{
    public class UpgradeNpc : Npc
    {
        [SerializeField] private int minUpgradeCnt, maxUpgradeCnt;
        [SerializeField] private UpgradeManagerSO upgradeManager;
        [SerializeField] private bool _isSale;
        [SerializeField] private float _minSalePercent;
        [SerializeField] private float _maxSalePercent;
        public UnityEvent OnTalkEvent;
        private List<UpgradeData> _shuffledDataList = new List<UpgradeData>();
        private bool _isEnd;
        
        protected override void Awake()
        {
            base.Awake();

            OnTalkCompleteEvent += HandleTalkComplete;
            
            UpgradeDataSO[] shuffleDataArr = upgradeManager.upgradeDataList.ToArray();
            Shuffle(100, shuffleDataArr);

            for(int i = 0; i < Random.Range(minUpgradeCnt, maxUpgradeCnt + 1); ++i)
                _shuffledDataList.Add(new UpgradeData()
                {
                    upgradeData = shuffleDataArr[i],
                    salePercent = Random.Range(_minSalePercent, _maxSalePercent)
                });
        }

        private void OnDestroy()
        {
            OnTalkCompleteEvent -= HandleTalkComplete;
        }

        public override void Hurt(int damage)
        {
            if(_isEnd)
                HandleTalkComplete();
            else
                base.Hurt(damage);
        }

        private void HandleTalkComplete()
        {
            OnTalkEvent?.Invoke();
            uiChannel.RaiseEvent(UIEvents.UpgradeUIPopUpEvent.Initializer(_shuffledDataList, _isSale));
            _isEnd = true;
            _player.SetEnableInput(false);
        }

        private void Shuffle(int cnt, UpgradeDataSO[] upgradeDataArr)
        {
            for (int i = 0; i < cnt; ++i)
            {
                int idx1 = Random.Range(0, upgradeDataArr.Length);
                int idx2 = Random.Range(0, upgradeDataArr.Length);

                (upgradeDataArr[idx1], upgradeDataArr[idx2]) = (upgradeDataArr[idx2], upgradeDataArr[idx1]);
            }
        }
    }
}