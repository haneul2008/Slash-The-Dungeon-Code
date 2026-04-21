using System;

namespace HN.Code.EventSystems
{
    public static class GoldEvents
    {
        public static readonly GoldChangeEvent GoldChangeEvent = new GoldChangeEvent();
    }

    public class GoldChangeEvent : GameEvent
    {
        public int gold;
        public Action<bool> onChangeGold;

        public GoldChangeEvent Initializer(int gold)
        {
            this.gold = gold;
            return this;
        }
    }
}