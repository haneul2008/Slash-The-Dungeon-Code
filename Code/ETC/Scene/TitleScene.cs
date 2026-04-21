using System;
using DG.Tweening;
using HN.Code.EventSystems;
using HN.Code.Managers;
using UnityEngine;

namespace HN.Code.ETC.Scene
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO sceneChannel;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private SceneDataSO gameSceneData;
        [SerializeField] private float sceneTransitionDelay;
        
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = CreateOnceManager.Instance.GetComponentInChildren<GameManager>();
        }

        public void HandleGameStart()
        {
            _gameManager.StartGame();
            uiChannel.RaiseEvent(UIEvents.FadeEvent.Initializer(false));
            DOVirtual.DelayedCall(sceneTransitionDelay, () =>
            {
                sceneChannel.RaiseEvent(SceneEvents.SceneChangeEvent.Initializer(gameSceneData));
            });
        }
    }
}