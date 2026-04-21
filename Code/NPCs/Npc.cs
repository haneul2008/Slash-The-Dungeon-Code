using System;
using HN.Code.EventSystems;
using HN.Code.Players;
using HN.Code.Reference.Texts;
using UnityEngine;

namespace HN.Code.NPCs
{
    public class Npc : MonoBehaviour, IDamageable
    {
        public event Action<string> OnTalkEvent;
        public event Action OnTalkCompleteEvent;
        
        [SerializeField] protected NpcDataSO npcData;
        [SerializeField] protected TextContainerSO textContainer;
        [SerializeField] protected GameEventChannelSO uiChannel;
        [SerializeField] protected PlayerFinderSO playerFinder;
        [SerializeField] protected Transform footTrm;
        
        protected int _textCnt;
        protected TextDataSO _textData;
        protected Player _player;
        private SpriteRenderer _renderer;

        protected virtual void Awake()
        {
            _textData = textContainer.GetTextData(npcData);
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _player = playerFinder.player;
        }

        public virtual void Talk()
        {
            OnTalkEvent?.Invoke(_textData.text[_textCnt]);
            
            _textCnt++;

            int textTotalCnt = _textData.text.Count;
            
            if (_textCnt == textTotalCnt)
            {
                OnTalkCompleteEvent?.Invoke();
                _textCnt = 0;
            }
        }

        private void OnValidate()
        {
            if (npcData == null) return;

            gameObject.name = $"{npcData.name}_NPC";
        }

        public virtual void Hurt(int damage)
        {
            _player.SetEnableInput(false);
            uiChannel.RaiseEvent(UIEvents.NpcTalkEvent.Initializer(this));
        }
    }
}