using HN.Code.Feedbacks;
using HN.Code.Items;
using UnityEngine;

namespace HN.Code.Test
{
    public class DropItemTester : MonoBehaviour
    {
        [SerializeField] private DropFeedback dropFeedback;
        
        [ContextMenu("DropItem")]
        public void DropItem()
        {
            dropFeedback.PlayFeedback();
        }
    }
}