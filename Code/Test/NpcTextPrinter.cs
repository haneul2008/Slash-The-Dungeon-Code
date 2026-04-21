using System;
using HN.Code.EventSystems;
using HN.Code.NPCs;
using UnityEngine;

namespace HN.Code.Test
{
    public class NpcTextPrinter : MonoBehaviour
    {
        [SerializeField] private Npc targetNpc;
        [SerializeField] private GameEventChannelSO uiChannel;

        private Npc _prev;

        private void Awake()
        {
            if (targetNpc != null)
            {
                targetNpc.OnTalkEvent += HandleTalk;
                targetNpc.OnTalkCompleteEvent += HandleTalkComplete;
            }
        }

        private void HandleTalkComplete() => print("talk complete");

        private void HandleTalk(string text) => print(text);

        [ContextMenu("Talk to npc")]
        public void Talk()
        {
            targetNpc?.Talk();
        }
        
        [ContextMenu("Talk start")]
        public void StartTalk()
        {
            uiChannel.RaiseEvent(UIEvents.NpcTalkEvent.Initializer(targetNpc));
        }

        private void Update()
        {
            if (_prev != null && _prev != targetNpc)
            {
                _prev.OnTalkEvent -= HandleTalk;
                _prev.OnTalkCompleteEvent -= HandleTalkComplete;
            }

            _prev = targetNpc;
        }
    }
}