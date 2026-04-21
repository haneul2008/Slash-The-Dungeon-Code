using System;
using HN.Code.EventSystems;
using UnityEngine;

namespace HN.Code.Test
{
    public class GoldTester : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO goldChannel;
        [SerializeField] private int targetGold;

        [ContextMenu("Change Gold")]
        public void ChangeGold()
        {
            goldChannel.RaiseEvent(GoldEvents.GoldChangeEvent.Initializer(targetGold));
        }
    }
}