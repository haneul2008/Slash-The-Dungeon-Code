using System;
using HN.Code.Stats;
using UnityEngine;

namespace HN.Code.Upgrades.Core
{
    public abstract class Upgrade
    {
        public UpgradeDataSO UpgradeData { get; protected set; }

        public Upgrade(UpgradeDataSO upgradeData)
        {
            UpgradeData = upgradeData;
        }
        
        public abstract void ApplyUpgrade(StatCompo statCompo);

        public abstract void CancelUpgrade(StatCompo statCompo);

        public abstract Upgrade GetUpgrade();
    }
}