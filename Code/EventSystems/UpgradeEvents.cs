using HN.Code.Upgrades.Core;

namespace HN.Code.EventSystems
{
    public static class UpgradeEvents
    {
        public static readonly ApplyUpgradeEvent ApplyUpgradeEvent = new ApplyUpgradeEvent();
        public static readonly RemoveUpgradeEvent RemoveUpgradeEvent = new RemoveUpgradeEvent();
    }

    public class ApplyUpgradeEvent : GameEvent
    {
        public UpgradeDataSO upgradeData;

        public ApplyUpgradeEvent Initializer(UpgradeDataSO upgradeData)
        {
            this.upgradeData = upgradeData;
            return this;
        }
    }
    
    public class RemoveUpgradeEvent : GameEvent
    {
        public UpgradeDataSO upgradeData;

        public RemoveUpgradeEvent Initializer(UpgradeDataSO upgradeData)
        {
            this.upgradeData = upgradeData;
            return this;
        }
    }
}