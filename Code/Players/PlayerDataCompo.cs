using System;
using System.Collections.Generic;
using HN.Code.EventSystems;
using HN.Code.Save;
using HN.Code.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Players
{
    public class PlayerDataCompo : MonoBehaviour, ISavable
    {
        [SerializeField] private StatCompo statCompo;
        [SerializeField] private PlayerHealth playerHealth;

        #region SaveData Logic

        [field: SerializeField] public SaveIdSO SaveID { get; private set; }

        [Serializable]
        public struct PlayerSaveData
        {
            public List<StatCompo.StatSaveData> stats;
            public int currentHealth;
        }

        public string GetSaveData()
        {
            PlayerSaveData data = new PlayerSaveData
            {
                stats = statCompo.GetSaveData(),
                currentHealth = playerHealth.GetSaveData()
            };
            return JsonUtility.ToJson(data);
        }

        public void RestoreData(string loadedData)
        {
            PlayerSaveData loadData = JsonUtility.FromJson<PlayerSaveData>(loadedData);

            if (loadData.stats != null)
                statCompo.RestoreData(loadData.stats);

            if (loadData.currentHealth != 0)
                playerHealth.RestoreData(loadData.currentHealth);
        }

        #endregion
    }
}