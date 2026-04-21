using System;
using DG.Tweening;
using HN.Code.Combat;
using HN.Code.Players;
using UnityEngine;

namespace HN.Code.UI
{
    public class BossHpUI : MonoBehaviour
    {
        [SerializeField] private Transform fillTrm;
        [SerializeField] private PlayerFinderSO playerFinder;
        [SerializeField] private Health bossHealth;
        [SerializeField] private Vector2 activePos;
        [SerializeField] private float activeDuration;

        private Vector2 _originPos;
        private RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
            _originPos = _rectTrm.anchoredPosition;
        }

        private void Start()
        {
            Health playerHealth = playerFinder.player.GetComponentInChildren<Health>();
            playerHealth.OnDead.AddListener(HandlePlayerDead);
        }

        private void HandlePlayerDead()
        {
            Health playerHealth = playerFinder.player.GetComponentInChildren<Health>();
            playerHealth.OnDead.RemoveListener(HandlePlayerDead);
            ActiveUI(false);
        }

        public void SetFill(int currentHp)
        {
            float fillAmount = currentHp / (float)bossHealth.MaxHealth;
            Vector3 fillScale = fillTrm.transform.localScale;
            fillTrm.transform.localScale = new Vector3(fillAmount, fillScale.y, 1);
        }

        public void ActiveUI(bool isActive)
        {
            Vector2 targetPos = isActive ? activePos : _originPos;
            _rectTrm.DOAnchorPos(targetPos, activeDuration);
        }
    }
}