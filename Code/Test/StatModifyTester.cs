using HN.Code.Stats;
using UnityEngine;

namespace HN.Code.Test
{
    public class StatModifyTester : MonoBehaviour
    {
        [SerializeField] private StatSO targetStat;
        [SerializeField] private StatCompo statCompo;
        [SerializeField] private float value;

        [ContextMenu("Print Modifier")]
        private void PrintModifier()
        {
            print(statCompo.GetStat(targetStat).Value);
        }

        [ContextMenu("Add modifier")]
        private void AddModifier()
        {
            statCompo.GetStat(targetStat).AddModifier("test", value);
        }
        
        [ContextMenu("Remove modifier")]
        private void RemoveModifier()
        {
            statCompo.GetStat(targetStat).RemoveModifier("test");
        }
    }
}