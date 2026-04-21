using HN.Code.Gold;
using HN.Code.Items;
using HN.HNLib.ObjectPool;
using UnityEngine;

namespace HN.Code.Feedbacks
{
    public class DropFeedback : Feedback
    {
        [SerializeField] private Transform dropTrm;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private DropTableSO dropTable;
        [SerializeField] private float dropPower;
        
        public override void PlayFeedback()
        {
            dropTable.tables.ForEach(DropItem);
        }

        private void DropItem(DropInfo info)
        {
            for (int i = 0; i < Random.Range(info.minSpawnAmount, info.maxSpawnAmount + 1); ++i)
            {
                if (info.dropRate > Random.value)
                {
                    Collectable item = poolManager.Pop<Collectable>(info.item.poolName);
                    item.SetItemData(info.item);
                    Vector2 randDirection = Random.insideUnitCircle.normalized;
                    item.DropIt(dropTrm.position, randDirection * dropPower);
                }
            }
        }

        public override void StopFeedback()
        {
        }
    }
}