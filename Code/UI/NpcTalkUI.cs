using CSI._01.Script.UI.Chat;
using DG.Tweening;
using HN.Code.EventSystems;
using HN.Code.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace HN.Code.UI
{
    public class NpcTalkUI : MonoBehaviour
    {
        [SerializeField] private RectTransform uiParentTrm;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private float activeDuration = 0.3f;
        [SerializeField] private Vector2 hidePos = new Vector2(0, -400);
        [SerializeField] private float clickDuration = 0.3f;
        
        private Npc _currentNpc;
        private ChatUI _chatUI;
        private Vector2 _originPos;
        private bool _isActive = false;
        private float _lastClickTime;
        
        private void Awake()
        {
            uiChannel.AddListener<NpcTalkEvent>(HandleNpcTalkStart);

            _chatUI = GetComponentInChildren<ChatUI>();
            _originPos = uiParentTrm.anchoredPosition;
            uiParentTrm.anchoredPosition = hidePos;
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<NpcTalkEvent>(HandleNpcTalkStart);
        }

        #region Temp code

        private void Update()
        {
            if (_isActive && Mouse.current.leftButton.wasPressedThisFrame && _lastClickTime + clickDuration < Time.time)
            {
                _currentNpc?.Talk();
                _lastClickTime = Time.time;
            }
        }

        #endregion

        private void HandleNpcTalkStart(NpcTalkEvent evt)
        {
            _currentNpc = evt.targetNpc;
            _currentNpc.OnTalkEvent += HandleTalk;
            _currentNpc.OnTalkCompleteEvent += HandleTalkComplete;
            
            _currentNpc.Talk();
            Active(true);
        }

        private void HandleTalkComplete()
        {
            Active(false);
        }

        private void HandleTalk(string text)
        {
            _chatUI.PlayText(text, string.Empty);
        }

        private void Active(bool isActive)
        {
            Vector2 targetPos = isActive ? _originPos : hidePos;
            uiParentTrm.DOAnchorPos(targetPos, activeDuration).SetEase(Ease.InQuad).OnComplete(() =>
            {
                _isActive = isActive;
                _lastClickTime = Time.time;
            });
        }
    }
}