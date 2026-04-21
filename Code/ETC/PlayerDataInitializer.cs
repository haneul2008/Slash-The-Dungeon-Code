using System;
using System.Collections.Generic;
using HN.Code.EventSystems;
using HN.Code.Players;
using HN.Code.Save;
using HN.Code.Stats;
using UnityEngine;

namespace HN.Code.ETC
{
    public class PlayerDataInitializer : MonoBehaviour, ISavable
    {
        [SerializeField] private GameEventChannelSO saveChannel;
        [SerializeField] private SaveIdSO playerSaveData;

        public SaveIdSO SaveID => playerSaveData;

        private void Start()
        {
            saveChannel.RaiseEvent(SaveEvents.SaveEvent.Initializer(false));
        }

        public string GetSaveData()
        {
            PlayerDataCompo.PlayerSaveData saveData = new PlayerDataCompo.PlayerSaveData()
            {
                stats = new List<StatCompo.StatSaveData>(),
                currentHealth = 7
            };

            return JsonUtility.ToJson(saveData);
        }

        public void RestoreData(string loadedData)
        {
        }
    }
}