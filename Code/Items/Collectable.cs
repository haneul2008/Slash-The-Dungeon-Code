using DG.Tweening;
using UnityEngine;

namespace HN.Code.Items
{
    public abstract class Collectable : MonoBehaviour
    {
        [SerializeField] protected ItemSO itemData;
        [SerializeField] protected float dropDelay = 0.2f;

        protected bool _alreadyCollected;
        protected Rigidbody2D _rigid;
        protected Collider2D _collider;
        protected bool _canCollectable;
        protected SpriteRenderer _spriteRenderer;
    
        protected virtual void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetItemData(ItemSO itemData)
        {
            this.itemData = itemData;
        }

        public void DropIt(Vector3 position, Vector2 force)
        {
            transform.position = position;
            _rigid.AddForce(force, ForceMode2D.Impulse);
            DOVirtual.DelayedCall(dropDelay, () => _canCollectable = true);
        }

        public abstract void Collect(Transform collector, float magneticPower);
    }
}