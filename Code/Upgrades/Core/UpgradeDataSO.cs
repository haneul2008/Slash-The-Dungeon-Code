using System;
using System.Collections.Generic;
using HN.Code.Stats;
using UnityEngine;

namespace HN.Code.Upgrades.Core
{
    [Serializable]
    public class UpgradeStat
    {
        public StatSO targetStat;
        public string key;
        public float value;
    }
    
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "SO/Upgrade/Data", order = 0)]
    public class UpgradeDataSO : ScriptableObject
    {
        public List<UpgradeStat> upgradeStats;
        public Sprite upgradeImage;
        public bool isValueUpgrade = true;
        public string upgradeName;
        public string className; //if isValue Upgrade == false
        public int requireGoldAmount;
        [TextArea] public string desc;

        private void OnValidate()
        {
            foreach (UpgradeStat stat in upgradeStats)
            {
                if (string.IsNullOrEmpty(stat.key))
                {
                    stat.key = Guid.NewGuid().ToString();
                }
            }
        }
    }
}