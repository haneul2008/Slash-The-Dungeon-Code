using System;
using HN.Code.EventSystems;
using UnityEngine;

namespace HN.Code.UI
{
    public class MinimapUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;

        private void Awake()
        {
            playerChannel.AddListener<PlayerDeadEvent>(HandlePlayerDead);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<PlayerDeadEvent>(HandlePlayerDead);
        }

        private void HandlePlayerDead(PlayerDeadEvent evt)
        {
            gameObject.SetActive(false);
        }
    }
}