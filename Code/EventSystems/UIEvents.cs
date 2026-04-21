using System.Collections.Generic;
using HN.Code.NPCs;
using HN.Code.UI;
using HN.Code.Upgrades.Core;

namespace HN.Code.EventSystems
{
    public static class UIEvents
    {
        public static readonly NpcTalkEvent NpcTalkEvent = new NpcTalkEvent();
        public static readonly BuyUpgradeEvent BuyUpgradeEvent = new BuyUpgradeEvent();
        public static readonly UpgradeUIPopUpEvent UpgradeUIPopUpEvent = new UpgradeUIPopUpEvent();
        public static readonly FadeEvent FadeEvent = new FadeEvent();
        public static readonly PopUpClearUIEvent PopUpClearUIEvent = new PopUpClearUIEvent();
    }

    public class NpcTalkEvent : GameEvent
    {
        public Npc targetNpc;

        public NpcTalkEvent Initializer(Npc npc)
        {
            this.targetNpc = npc;
            return this;
        }
    }

    public class BuyUpgradeEvent : GameEvent
    {
        public UpgradeUI upgradeUI;
        public bool isSale;
        public float salePercent;

        public BuyUpgradeEvent Initializer(UpgradeUI upgradeUI, bool isSale, float salePercent)
        {
            this.upgradeUI = upgradeUI;
            this.isSale = isSale;
            this.salePercent = salePercent;
            return this;
        }
    }

    public struct UpgradeData
    {
        public UpgradeDataSO upgradeData;
        public float salePercent;
    }

    public class UpgradeUIPopUpEvent : GameEvent
    {
        public List<UpgradeData> upgradeDataList;
        public bool isSale;

        public UpgradeUIPopUpEvent Initializer(List<UpgradeData> upgradeDataList, bool isSale)
        {
            this.upgradeDataList = upgradeDataList;
            this.isSale = isSale;
            return this;
        }
    }

    public class FadeEvent : GameEvent
    {
        public bool isActive;

        public FadeEvent Initializer(bool isActive)
        {
            this.isActive = isActive;
            return this;
        }
    }

    public class PopUpClearUIEvent : GameEvent
    {
    }
}