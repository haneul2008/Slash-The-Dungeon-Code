using System;
using System.Collections.Generic;
using System.Linq;
using HN.Code.EventSystems;
using HN.Code.Save;
using UnityEngine;

namespace HN.Code.Managers
{
    [Serializable]
    public class SaveData
    {
        public int saveId;
        public string data;
    }
    
    [Serializable]
    public struct DataCollection
    {
        public List<SaveData> dataList;
    }
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO saveChannel;
        [SerializeField] private string saveDataKey = "savedGame";
        
        private List<SaveData> _unUsedData = new List<SaveData>();

        private void Awake()
        {
            saveChannel.AddListener<SaveEvent>(HandleSave);
            saveChannel.AddListener<LoadEvent>(HandleLoad);
        }

        private void OnDestroy()
        {
            saveChannel.RemoveListener<SaveEvent>(HandleSave);
            saveChannel.RemoveListener<LoadEvent>(HandleLoad);
        }

        private void HandleSave(SaveEvent evt)
        {
            if (evt.isSaveToFile == false)
                SaveGameToPrefs();
        }
        
        private void SaveGameToPrefs()
        {
            string dataJson = GetDataToSave();
            PlayerPrefs.SetString(saveDataKey, dataJson);
        }

        private string GetDataToSave()
        {
            IEnumerable<ISavable> savableObjects =
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>();
            
            List<SaveData> saveDataList = new List<SaveData>();

            foreach (ISavable savable in savableObjects)
            {
                saveDataList.Add(new SaveData{saveId = savable.SaveID.saveID, data = savable.GetSaveData()});
            }
            
            saveDataList.AddRange(_unUsedData);
            DataCollection saveDataCollection = new DataCollection { dataList = saveDataList };
            
            return JsonUtility.ToJson(saveDataCollection);
        }

        private void HandleLoad(LoadEvent evt)
        {
            if (evt.isLoadFromFile == false)
                LoadFromPrefs();
        }
        
        private void LoadFromPrefs()
        {
            string loadedJson = PlayerPrefs.GetString(saveDataKey, string.Empty);
            RestoreData(loadedJson);
        }

        private void RestoreData(string loadedJson)
        {
            //해당 씬에서 데이터를 받아줄 오브젝트를 다 가져온다.
            IEnumerable<ISavable> savableObjects 
                = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>();
            
            DataCollection collection = string.IsNullOrEmpty(loadedJson) 
                ? new DataCollection() : JsonUtility.FromJson<DataCollection>(loadedJson);

            _unUsedData.Clear(); //현재 가지고 있는 미사용데이터는 클리어.

            if (collection.dataList != null) //로드된 데이터가 존재한다면
            {
                foreach (SaveData saveData in collection.dataList)
                {
                    ISavable target = savableObjects.FirstOrDefault(savable => savable.SaveID.saveID == saveData.saveId);

                    if (target != default)
                    {
                        target.RestoreData(saveData.data);
                    }
                    else
                    {
                        _unUsedData.Add(saveData);
                    }
                }
            }
        }
    }
}