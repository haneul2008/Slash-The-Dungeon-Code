using System;
using System.Collections.Generic;
using UnityEngine;

namespace HN.Code.Items
{
    [Serializable]
    public struct DropInfo
    {
        public ItemSO item;
        public float dropRate;
        public int minSpawnAmount, maxSpawnAmount;
    }

    [CreateAssetMenu(menuName = "SO/Item/Table")]
    public class DropTableSO : ScriptableObject
    {
        public List<DropInfo> tables;
    }
}