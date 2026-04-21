using HN.Code.Upgrades.Core;
using UnityEngine;

namespace HN.Code.EventSystems
{
    public static class PlayerEvents
    {
        public static readonly PlayerMoveEvent PlayerMoveEvent = new PlayerMoveEvent();
        public static readonly SuccessBuyUpgradeEvent SuccessBuyUpgradeEvent = new SuccessBuyUpgradeEvent();
        public static readonly PlayerDeadEvent PlayerDeadEvent = new PlayerDeadEvent();
        public static readonly PlayerHitEvent PlayerHitEvent = new PlayerHitEvent();
        public static readonly PlayerHealthResetEvent PlayerHealthResetEvent = new PlayerHealthResetEvent();
    }

    public class PlayerMoveEvent : GameEvent
    {
        public Vector2 pos;

        public PlayerMoveEvent Initializer(Vector2 pos)
        {
            this.pos = pos;
            return this;
        }
    }

    public class SuccessBuyUpgradeEvent : GameEvent
    {
        public UpgradeDataSO upgradeData;

        public SuccessBuyUpgradeEvent Initialzier(UpgradeDataSO upgradeData)
        {
            this.upgradeData = upgradeData;
            return this;
        }
    }

    public class PlayerDeadEvent : GameEvent
    {
    }

    public class PlayerHitEvent : GameEvent
    {
        public int hp;

        public PlayerHitEvent Initializer(int hp)
        {
            this.hp = hp;
            return this;
        }
    }
    
    public class PlayerHealthResetEvent : GameEvent
    {
        public int hp;
        public int maxHp;

        public PlayerHealthResetEvent Initializer(int hp, int maxHp)
        {
            this.hp = hp;
            this.maxHp = maxHp;
            return this;
        }
    }
}