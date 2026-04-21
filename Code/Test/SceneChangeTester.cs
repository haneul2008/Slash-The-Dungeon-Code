using System;
using HN.Code.ETC.Scene;
using HN.Code.EventSystems;
using UnityEngine;

namespace HN.Code.Test
{
    public class SceneChangeTester : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO sceneChannel;
        [SerializeField] private SceneDataSO sceneData;

        [ContextMenu("Change scene")]
        public void ChangeScene()
        {
            sceneChannel.RaiseEvent(SceneEvents.SceneChangeEvent.Initializer(sceneData));
        }
    }
}