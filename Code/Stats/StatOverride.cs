using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Stats
{
    [Serializable]
    public class StatOverride
    {
        [SerializeField] private StatSO stat;
        [SerializeField] private bool isUseOverrideValue;
        [SerializeField] private float overrideValue;
        [SerializeField] private bool isUseOverrideMinMax;
        [SerializeField] private float overrideMinValue, overrideMaxValue;
        
        public StatOverride(StatSO stat) => this.stat = stat;

        public StatSO CreateStat()
        {
            StatSO newStat = stat.Clone() as StatSO;
            Debug.Assert(newStat != null, $"{stat.statName} clone failed");

            if (isUseOverrideValue)
                newStat.BaseValue = overrideValue;

            if (isUseOverrideMinMax)
            {
                newStat.minValue = overrideMinValue;
                newStat.maxValue = overrideMaxValue;
            }
            
            return newStat;
        }
    }
}