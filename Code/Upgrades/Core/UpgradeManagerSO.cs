using System;
using System.Collections.Generic;
using System.Linq;
using HN.Code.Stats;
using UnityEngine;

namespace HN.Code.Upgrades.Core
{
    [CreateAssetMenu(fileName = "UpgradeManager", menuName = "SO/Upgrade/UpgradeManager", order = 0)]
    public class UpgradeManagerSO : ScriptableObject
    {
        public List<UpgradeDataSO> upgradeDataList = new List<UpgradeDataSO>();
        private readonly List<Upgrade> _upgradeFactory = new List<Upgrade>();

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            _upgradeFactory.Clear();
            
            foreach (UpgradeDataSO upgradeData in upgradeDataList)
            {
                Type type;

                if (upgradeData.isValueUpgrade)
                    type = typeof(ValueUpgradable);
                else
                {
                    type = Type.GetType(upgradeData.className);
                    if (type == null || (!type.IsSubclassOf(typeof(Upgrade)) && !type.IsAbstract))
                    {
                        Debug.LogWarning($"type is wrong : {upgradeData}");;
                        continue;
                    }
                }

                Upgrade upgrade = Activator.CreateInstance(type, new object[] {upgradeData} ) as Upgrade;
                _upgradeFactory.Add(upgrade);
            }
        }

        public Upgrade CreateUpgrade(UpgradeDataSO upgradeData)
        {
            Upgrade targetUpgrade = _upgradeFactory.FirstOrDefault(upgrade => upgrade.UpgradeData == upgradeData);
            return targetUpgrade?.GetUpgrade();
        }

        public UpgradeDataSO GetUpgradeData(string upgradeName)
        {
            foreach (UpgradeDataSO upgradeData in upgradeDataList)
            {
                if (upgradeData.name == upgradeName)
                    return upgradeData;
            }

            return null;
        }
    }
}