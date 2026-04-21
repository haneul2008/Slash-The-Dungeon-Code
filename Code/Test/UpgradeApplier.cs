using HN.Code.Upgrades.Core;
using UnityEngine;

namespace HN.Code.Test
{
    public class UpgradeApplier : MonoBehaviour
    {
        [SerializeField] private UpgradeDataSO upgradeData;
        [SerializeField] private PlayerUpgradeCompo upgradeCompo;

        [ContextMenu("Apply Upgrade")]
        private void ApplyUpgrade()
        {
            upgradeCompo.ApplyUpgrade(upgradeData);
        }
        
        [ContextMenu("Cancel Upgrade")]
        private void CancelUpgrade()
        {
            upgradeCompo.CancelUpgrade(upgradeData);
        }
    }
}