using HN.Code.EventSystems;
using UnityEngine;

namespace HN.Code.UI
{
    public class RollStatUI : StatUI
    {
        private PlayerMove _playerMove;

        protected override void Start()
        {
            base.Start();

            _playerMove = playerFinder.player.GetComponent<PlayerMove>();
            _playerMove.OnRollEndEvent += SetFill;
        }

        protected override void HandleFade(FadeEvent evt)
        {
            base.HandleFade(evt);
            
            _playerMove.OnRollEndEvent -= SetFill;
        }
    }
}