using System;
using System.Collections.Generic;
using System.Linq;
using HN.Code.EventSystems;
using HN.Code.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Upgrades.Core
{
    public class PlayerUpgradeCompo : MonoBehaviour
    {
        public List<Upgrade> CurrentUpgrades { get; private set; } = new List<Upgrade>();

        [SerializeField] private UpgradeManagerSO upgradeManager;
        [SerializeField] private GameEventChannelSO upgradeChannel;
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private StatCompo statCompo;

        private void Awake()
        {
            playerChannel.AddListener<SuccessBuyUpgradeEvent>(HandleBuyUpgrade);
        }

        private void OnDestroy()
        {
            CurrentUpgrades.ForEach(upgrade => upgrade.CancelUpgrade(statCompo));
            playerChannel.RemoveListener<SuccessBuyUpgradeEvent>(HandleBuyUpgrade);
        }

        private void HandleBuyUpgrade(SuccessBuyUpgradeEvent evt)
        {
            ApplyUpgrade(evt.upgradeData);
        }

        public void ApplyUpgrade(UpgradeDataSO upgradeData)
        {
            Upgrade newUpgrade = upgradeManager.CreateUpgrade(upgradeData);
            newUpgrade.ApplyUpgrade(statCompo);
            CurrentUpgrades.Add(newUpgrade);
            upgradeChannel.RaiseEvent(UpgradeEvents.ApplyUpgradeEvent.Initializer(upgradeData));
        }

        public void CancelUpgrade(UpgradeDataSO upgradeData)
        {
            Upgrade targetUpgrade = CurrentUpgrades.FirstOrDefault(upgrade => upgrade.UpgradeData == upgradeData);
            if (targetUpgrade == null) return;

            targetUpgrade.CancelUpgrade(statCompo);
            CurrentUpgrades.Remove(targetUpgrade);
            upgradeChannel.RaiseEvent(UpgradeEvents.RemoveUpgradeEvent.Initializer(upgradeData));
        }
    }
}