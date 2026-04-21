using System;
using HN.Code.Combat;
using HN.Code.ETC;
using Unity.Cinemachine;
using UnityEngine;

namespace HN.Code.Stages
{
    public class BossStage : Stage
    {
        [SerializeField] private Health bossHealth;
        [SerializeField] private Collider2D cameraConfiner;
        [SerializeField] private BossRoomTrigger roomTrigger;

        private CinemachineCamera _followCam;
        
        private void Awake()
        {
            cameraConfiner.enabled = false;
            _followCam = FindAnyObjectByType<CinemachineCamera>();

            roomTrigger.OnTriggered.AddListener(HandleRoomTriggered);
            bossHealth.OnDead.AddListener(HandleDead);
        }

        private void OnDestroy()
        {
            roomTrigger.OnTriggered.AddListener(HandleRoomTriggered);
            bossHealth.OnDead.RemoveListener(HandleDead);
        }

        private void HandleRoomTriggered()
        {
            CinemachineConfiner2D confiner2D = _followCam.GetComponent<CinemachineConfiner2D>();
            confiner2D.BoundingShape2D = cameraConfiner;
            cameraConfiner.enabled = true;
        }

        private void HandleDead()
        {
            
        }
    }
}