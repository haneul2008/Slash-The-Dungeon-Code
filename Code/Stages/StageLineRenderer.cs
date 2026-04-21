using HN.Code.EventSystems;
using HN.Code.UI;
using UnityEngine;

namespace HN.Code.Stages
{
    public class StageLineRenderer : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO stageChannel;
        [SerializeField] private UILineRenderer lineRenderer;

        private void Awake()
        {
            stageChannel.AddListener<DrawLineEvent>(HandleMapLoad);
        }

        private void OnDestroy()
        {
            stageChannel.RemoveListener<DrawLineEvent>(HandleMapLoad);
        }

        private void HandleMapLoad(DrawLineEvent evt)
        {
            lineRenderer.points = new Vector2[evt.points.Count];

            for (int i = 0; i < evt.points.Count; ++i)
            {
                lineRenderer.points[i] = evt.points[i];
            }
        }
    }
}