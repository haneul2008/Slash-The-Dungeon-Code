using System;
using System.Collections.Generic;
using HN.Code.Reference.Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Reference
{
    [Serializable]
    public class GameData
    {
        public List<string> upgradeNames;
        public StageData StageData;
        public int stageId;
        public Vector2 playerPos;
        public float masterVolume;
        public float bgmVolume;
        public float soundVolume;
    }
}