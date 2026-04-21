namespace HN.Code.EventSystems
{
    public static class GameEvents
    {
        public static readonly GameStartEvent GameStartEvent = new GameStartEvent();
    }

    public class GameStartEvent : GameEvent
    {
    }
}