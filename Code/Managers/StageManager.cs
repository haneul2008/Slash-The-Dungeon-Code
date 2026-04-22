using System;
using System.Collections.Generic;
using System.Linq;
using HN.Code.EventSystems;
using HN.Code.Stages;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace HN.Code.Managers
{
    public class MapTree
    {
        public StageDataSO stageData;

        public MapTree left;
        public MapTree right;
    }

    public class StageManager : MonoBehaviour
    {
        [SerializeField] private StageListSO stageList;
        [SerializeField] private int stageLength;
        [SerializeField] private GameEventChannelSO stageChannel;
        [SerializeField] private StageDataSO initStage;
        [SerializeField] private StageDataSO bossStage;
        [SerializeField] private List<Stage> stages;

        public StageDataSO CurrentStageData { get; private set; }
        public MapTree CurrentMapTree { get; private set; }
        public MapTree MapRoot { get; private set; }
        public int StageDepth { get; private set; } = 0;
        private readonly Dictionary<StageDataSO, List<Stage>> _stagePairs = new Dictionary<StageDataSO, List<Stage>>();
        private List<StageDataSO> _stageList = new List<StageDataSO>();

        private void Awake()
        {
            stages.ForEach(stage =>
            {
                if (_stagePairs.TryGetValue(stage.StageData, out List<Stage> stageValue))
                {
                    stageValue.Add(stage);
                }
                else
                {
                    _stagePairs.Add(stage.StageData, new List<Stage>() { stage });
                }
            });

            _stageList = stageList.stageDataList.Where(data => data != bossStage).ToList();
            
            stageChannel.AddListener<StageSpawnEvent>(HandleStageSpawn);
            stageChannel.AddListener<StageSelectEvent>(HandleStageSelect);
            stageChannel.AddListener<StageInitEvent>(HandleStageInit);
        }

        private void OnDestroy()
        {
            stageChannel.RemoveListener<StageSpawnEvent>(HandleStageSpawn);
            stageChannel.RemoveListener<StageSelectEvent>(HandleStageSelect);
            stageChannel.RemoveListener<StageInitEvent>(HandleStageInit);
        }

        private void HandleStageInit(StageInitEvent evt)
        {
            CreateMap();
        }

        public void CreateMap()
        {
            MapRoot = SetMapNode(null, stageLength);
            StageDepth = 0;
            CurrentMapTree = MapRoot;
            CurrentStageData = MapRoot.stageData;
        }

        private MapTree SetMapNode(StageDataSO excludeData, int length)
        {
            if (length == 0)
            {
                return new MapTree()
                {
                    stageData = bossStage,
                    left = null,
                    right = null
                };
            }

            MapTree node = new MapTree();
            if (length == stageLength)
                node.stageData ??= initStage;
            else
                node.stageData ??= GetRandomStageData(excludeData);

            node.left = SetMapNode(null, length - 1);
            node.right = SetMapNode(node.left?.stageData, length - 1);

            return node;
        }
        
        private StageDataSO GetRandomStageData(StageDataSO excludeData)
        {
            float maxValue = excludeData != null ? 100 - excludeData.stagePercentage : 100;
            float targetPercent = Random.Range(0, maxValue);

            float sum = 0;

            foreach (StageDataSO data in _stageList)
            {
                if (data == excludeData) continue;

                sum += data.stagePercentage;

                if (sum > targetPercent)
                    return data;
            }

            return null;
        }
        
        private void HandleStageSpawn(StageSpawnEvent evt)
        {
            if (CurrentStageData == null) return;

            if (_stagePairs.TryGetValue(CurrentStageData, out List<Stage> stageValue))
            {
                int randNum = Random.Range(0, stageValue.Count);
                Stage targetStage = stageValue[randNum];
                Stage spawnedStage = Instantiate(targetStage);
                spawnedStage.InitStage();
            }
        }
        
        private void HandleStageSelect(StageSelectEvent evt)
        {
            //CurrentStageData = evt.tempData;
            SelectStage(evt.isRight);
        }

        public void SelectStage(bool isRight)
        {
            MapTree nextStage = isRight ? CurrentMapTree.right : CurrentMapTree.left;
            
            if(nextStage == null) return;

            CurrentMapTree = nextStage;
            CurrentStageData = nextStage.stageData;
            
            stageChannel.RaiseEvent(StageEvents.StageChangeEvent.Initiailzier(++StageDepth));
        }
    }
}