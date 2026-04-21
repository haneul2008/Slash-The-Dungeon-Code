using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Stats
{
    public class StatCompo : MonoBehaviour
    {
        [SerializeField] private List<StatOverride> statOverrides;

        private StatSO[] _stats;

        private void Awake()
        {
            _stats = statOverrides.Select(stat => stat.CreateStat()).ToArray();
        }

        public StatSO GetStat(StatSO targetStat) => _stats.FirstOrDefault(stat => stat.statName == targetStat.statName);
        
        [Serializable]
        public struct StatSaveData
        {
            public string statName;
            public float baseValue;
            [FormerlySerializedAs("modifyKeys")] public List<ModifyData> modifyDatas;
        }
        
        [Serializable]
        public struct ModifyData
        {
            public string key;
            public float value;
        }
        
        public List<StatSaveData> GetSaveData()
            => _stats.Aggregate(new List<StatSaveData>(), (saveList, stat) =>
            {
                saveList.Add(new StatSaveData
                {
                    statName = stat.statName, baseValue = stat.BaseValue,
                    modifyDatas = stat.GetModifyData()
                });
                return saveList;
            });
        
        public void RestoreData(List<StatSaveData> loadedDataList)
        {
            foreach (StatSaveData loadData in loadedDataList)
            {
                StatSO targetStat = _stats.FirstOrDefault(stat => stat.statName == loadData.statName);
                if (targetStat != default)
                {
                    targetStat.BaseValue = loadData.baseValue;
                }

                foreach (ModifyData modifyData in loadData.modifyDatas)
                {
                    targetStat?.AddModifier(modifyData.key, modifyData.value);
                }
            }
        }
    }
}