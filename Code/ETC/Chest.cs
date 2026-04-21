using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace HN.Code.ETC
{
    public class Chest : MonoBehaviour, IDamageable
    {
        public UnityEvent OnOpenEvent;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private float spawnRate = 0.7f;

        private bool _isOpened;

        private void Awake()
        {
            float rate = Random.value;
            
            if(spawnRate < rate)
                Destroy(gameObject);
        }

        public void Hurt(int damage)
        {
            if(_isOpened) return;

            _isOpened = true;

            spriteRenderer.sprite = openSprite;
            
            OnOpenEvent?.Invoke();
        }
    }
}