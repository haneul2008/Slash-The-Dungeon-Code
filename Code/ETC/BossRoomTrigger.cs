using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace HN.Code.ETC
{
    public class BossRoomTrigger : MonoBehaviour
    {
        public UnityEvent OnTriggered;
        
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private GameObject entranceObject;

        private bool _isTriggered;

        private void Awake()
        {
            entranceObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTriggered == false && (1 << other.gameObject.layer & whatIsPlayer) > 0)
            {
                OnTriggered?.Invoke();
                entranceObject.SetActive(true);
                _isTriggered = true;
            }
        }
    }
}