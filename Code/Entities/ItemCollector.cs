using HN.Code.Gold;
using HN.Code.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Entities
{
    public class ItemCollector : MonoBehaviour
    {
        [SerializeField] private float magneticPower = 1f;
        [SerializeField] private float radius = 1.5f;
        [SerializeField] private ContactFilter2D contactFilter2D;
        [SerializeField] private int maxDetectCount = 10;

        private Collider2D[] _collectArray;

        private void Awake()
        {
            _collectArray = new Collider2D[maxDetectCount];
        }

        private void FixedUpdate()
        {
            int count = Physics2D.OverlapCircle(transform.position, radius, contactFilter2D, _collectArray);

            for (int i = 0; i < count; i++)
            {
                if (_collectArray[i].TryGetComponent(out Collectable collectable))
                {
                    collectable.Collect(transform, magneticPower);
                }
            }
        }
    }
}