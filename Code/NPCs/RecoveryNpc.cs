using System;
using System.Collections;
using HN.Code.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.NPCs
{
    public class RecoveryNpc : Npc
    {
        [SerializeField] private float recoveryDuration;
        [SerializeField] private int recoveryAmount;
        [SerializeField] private int recoveryTextCnt = 2;

        private bool _canTalk = true;
        private bool _isEnd = false;

        protected override void Awake()
        {
            base.Awake();

            OnTalkCompleteEvent += HandleTalkComplete;
        }

        private void OnDestroy()
        {
            OnTalkCompleteEvent -= HandleTalkComplete;
        }

        public override void Talk()
        {
            if (_canTalk == false) return;
            
            base.Talk();

            if (_textCnt == recoveryTextCnt)
            {
                _player.PlayBuffEffect(BuffType.Recovery);
                _canTalk = false;
                StartCoroutine(RecoveryCoroutine());
            }
        }

        private IEnumerator RecoveryCoroutine()
        {
            yield return new WaitForSeconds(recoveryDuration);
            _player.Hurt(-recoveryAmount);
            _canTalk = true;
            Talk();
        }

        public override void Hurt(int damage)
        {
            if (_isEnd) return;
            
            base.Hurt(damage);
        }

        private void HandleTalkComplete()
        {
            _player.SetEnableInput(true);
            _isEnd = true;
        }
    }
}