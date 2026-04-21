using System;
using System.Collections.Generic;
using DG.Tweening;
using HN.Code.ETC.Scene;
using HN.Code.EventSystems;
using HN.Code.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace HN.Code.UI
{
    public class StageChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO stageChannel;
        [SerializeField] private GameEventChannelSO sceneChannel;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private SceneDataSO gameSceneData;
        [SerializeField] private MapBt rootMapBt;
        [SerializeField] private Transform playerVisualTrm;
        [SerializeField] private Transform contentTrm;
        [SerializeField] private GameObject descPanel;
        [SerializeField] private float viewOffsetY;
        [SerializeField] private float moveDuration;
        [SerializeField] private float sceneTransitionDelay;

        private readonly List<MapBt> _renderTable = new List<MapBt>();
        private StageManager _stageManager;
        private MapBt _currentMapBt;
        private bool? _isRight;
        private bool _isSelected;
        private Tween _moveTween;

        private void Start()
        {
            _stageManager = CreateOnceManager.Instance.GetComponentInChildren<StageManager>();
            List<Vector2> points = new List<Vector2>();
            InitVisuals(_stageManager.MapRoot, points, rootMapBt);
            SetPlayerPos(_stageManager.MapRoot, _stageManager.CurrentMapTree, rootMapBt);
            stageChannel.RaiseEvent(StageEvents.DrawLineEvent.Initializer(points));
            
            descPanel.SetActive(_stageManager.StageDepth == 0);
        }

        private void Update()
        {
            if (_currentMapBt is null) return;

            if (Keyboard.current.aKey.wasPressedThisFrame)
                MovePlayer(false);
            if (Keyboard.current.dKey.wasPressedThisFrame)
                MovePlayer(true);
            if (Keyboard.current.sKey.wasPressedThisFrame)
                SelectStage();
        }

        private void SelectStage()
        {
            if (_moveTween != null || _isRight.HasValue == false || _isSelected) return;

            _isSelected = true;
            
            uiChannel.RaiseEvent(UIEvents.FadeEvent.Initializer(false));
            DOVirtual.DelayedCall(sceneTransitionDelay, () =>
            {
                stageChannel.RaiseEvent(StageEvents.StageSelectEvent.Initializer(_isRight.Value));
                sceneChannel.RaiseEvent(SceneEvents.SceneChangeEvent.Initializer(gameSceneData));
            });
        }

        private void MovePlayer(bool isRight)
        {
            if (_moveTween != null || _currentMapBt.Left is null || _currentMapBt.Right is null) return;

            Vector2 currentPos = _currentMapBt.transform.position;
            Vector2 targetPos =
                isRight ? _currentMapBt.Right.transform.position : _currentMapBt.Left.transform.position;

            if (_isRight.HasValue && _isRight.Value != isRight && _currentMapBt.Right.IsBossStage == false)
                MoveToTargetPos(currentPos, () => MoveToTargetPos(targetPos, () =>
                {
                    _moveTween = null;
                    _isRight = isRight;
                }));
            else
                MoveToTargetPos(targetPos, () =>
                {
                    _moveTween = null;
                    _isRight = isRight;
                });
        }

        private void MoveToTargetPos(Vector2 targetPos, Action onComplete)
        {
            _moveTween = playerVisualTrm.DOMove(targetPos, moveDuration).OnComplete(() => onComplete?.Invoke());
        }

        private void SetPlayerPos(MapTree currentTree, MapTree targetTree, MapBt mapBt)
        {
            if (currentTree == null) return;

            if (currentTree == targetTree)
            {
                playerVisualTrm.position = mapBt.transform.position;
                float distanceY = contentTrm.position.y - mapBt.transform.position.y;
                contentTrm.position = new Vector3(contentTrm.position.x, contentTrm.position.y + distanceY + viewOffsetY);
                _currentMapBt = mapBt;
                _isRight = null;
                return;
            }

            SetPlayerPos(currentTree.left, targetTree, mapBt.Left);
            SetPlayerPos(currentTree.right, targetTree, mapBt.Right);
        }

        private void InitVisuals(MapTree mapTree, List<Vector2> points, MapBt mapBt)
        {
            if (mapTree == null || _renderTable.Contains(mapBt))
                return;

            Vector2 currentPos = mapBt.transform.position;

            points.Add(currentPos);
            _renderTable.Add(mapBt);

            mapBt.SetMap(mapTree.stageData);

            if (mapTree.left != null)
            {
                InitVisuals(mapTree.left, points, mapBt.Left);
                points.Add(mapBt.Left.transform.position);
                points.Add(currentPos);
            }

            if (mapTree.right != null)
            {
                InitVisuals(mapTree.right, points, mapBt.Right);
                points.Add(mapBt.Right.transform.position);
            }
        }
    }
}