using HN.Code.Stats;
using HN.Code.Upgrades.Core;
using UnityEngine;

namespace HN.Code.Upgrades
{
    public class ValueUpgradable : Upgrade
    {
        public ValueUpgradable(UpgradeDataSO upgradeData) : base(upgradeData)
        {
        }

        public override void ApplyUpgrade(StatCompo statCompo)
        {
            foreach (UpgradeStat upgradeStat in UpgradeData.upgradeStats)
            {
                StatSO stat = statCompo.GetStat(upgradeStat.targetStat);
                stat.AddModifier(upgradeStat.key, upgradeStat.value);
            }
        }

        public override void CancelUpgrade(StatCompo statCompo)
        {
            foreach (UpgradeStat upgradeStat in UpgradeData.upgradeStats)
            {
                StatSO stat = statCompo.GetStat(upgradeStat.targetStat);
                stat.RemoveModifier(upgradeStat.key);
            }
        }

        public override Upgrade GetUpgrade() => this;
    }
}