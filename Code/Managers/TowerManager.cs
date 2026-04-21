using System;
using System.Linq;
using HN.Code.Players;
using HN.Code.Towers;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Managers
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private PlayerFinderSO playerFinder;
        
        private void Start()
        {
            Tower[] towers = FindObjectsByType<Tower>(FindObjectsSortMode.None);
            towers.ToList().ForEach(tower => tower.Initialize(playerFinder.player));
        }
    }
}