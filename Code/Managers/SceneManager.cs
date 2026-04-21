using System;
using System.Collections;
using System.Collections.Generic;
using HN.Code.ETC.Scene;
using HN.Code.EventSystems;
using HN.Code.Reference;
using HN.Code.Save;
using HN.Code.Upgrades.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HN.Code.Managers
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO sceneChannel;
        [SerializeField] private GameEventChannelSO saveChannel;

        private SceneDataSO _currentSceneData;
        
        private void Awake()
        {
            sceneChannel.AddListener<SceneChangeEvent>(HandleSceneChange);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDestroy()
        {
            sceneChannel.RemoveListener<SceneChangeEvent>(HandleSceneChange);
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(LoadSceneCoroutine(10));
        }

        private IEnumerator LoadSceneCoroutine(int frame)
        {
            for (int i = 0; i < frame; ++i)
            {
                yield return null;
            }
            
            if (_currentSceneData == null) yield break;

            Time.timeScale = 1;
            saveChannel.RaiseEvent(SaveEvents.LoadEvent.Initializer(false));
        }

        private void HandleSceneChange(SceneChangeEvent evt)
        {
            SceneDataSO sceneData = evt.sceneData;
            if (sceneData.isSavePlayerData)
            {
                saveChannel.RaiseEvent(SaveEvents.SaveEvent.Initializer(false));
            }
            
            _currentSceneData = sceneData;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneData.sceneName);
        }
    }
}