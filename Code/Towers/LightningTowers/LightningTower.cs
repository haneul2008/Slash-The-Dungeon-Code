using System;
using System.Collections.Generic;
using DG.Tweening;
using HN.Code.ETC.WarningObjects;
using HN.HNLib.ObjectPool;
using UnityEngine;
using UnityEngine.Events;

namespace HN.Code.Towers.LightningTowers
{
    public class LightningTower : Tower
    {
        public UnityEvent OnSpawnLightning;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private WarningObejctDataSO warningObjectData;
        [SerializeField] private float feedbackDistance = 5f;
        [SerializeField] private float attackDistance;
        [SerializeField] private int attackAmount;
        [SerializeField] private int damage;

        private List<Vector2> attackPosList = new List<Vector2>();

        public override bool CanAttack() => true;

        public override void Attack()
        {
            if (attackPosList.Count == 0)
            {
                float angle = 360f / attackAmount;
                for (int i = 0; i < attackAmount; ++i)
                {
                    float rad = angle * i * Mathf.Deg2Rad;
                    Vector2 targetPos = floorTrm.position + new Vector3(Mathf.Cos(rad), + Mathf.Sin(rad)) * attackDistance;
                
                    attackPosList.Add(targetPos);
                
                    SpawnWarningObject(targetPos);
                }
            }
            else
            {
                foreach (Vector2 attackPos in attackPosList)
                    SpawnWarningObject(attackPos);
            }

            DOVirtual.DelayedCall(warningObjectData.duration, () =>
            {
                if(Vector3.Distance(transform.position, _target.transform.position) < feedbackDistance)
                    OnSpawnLightning?.Invoke();
            });
        }

        private void SpawnWarningObject(Vector2 targetPos)
        {
            WarningObject warningObject = poolManager.Pop<WarningObject>();
            warningObject.SetUp(targetPos, warningObjectData, SpawnLightning);
        }

        private void SpawnLightning(Vector2 targetPos)
        {   
            Lightning lightning = poolManager.Pop<Lightning>();

            lightning.SetUp(targetPos, damage);
        }
    }
}