using System.Collections.Generic;
using HN.Code.Stages;
using UnityEngine;

namespace HN.Code.EventSystems
{
    public static class StageEvents
    {
        public static readonly DrawLineEvent DrawLineEvent = new DrawLineEvent();
        public static readonly PlayerSpawnEvent PlayerSpawnEvent = new PlayerSpawnEvent();
        public static readonly StageSpawnEvent StageSpawnEvent = new StageSpawnEvent();
        public static readonly StageSelectEvent StageSelectEvent = new StageSelectEvent();
        public static readonly StageChangeEvent StageChangeEvent = new StageChangeEvent();
        public static readonly StageInitEvent StageInitEvent = new StageInitEvent();
    }

    public class DrawLineEvent : GameEvent
    {
        public List<Vector2> points;

        public DrawLineEvent Initializer(List<Vector2> points)
        {
            this.points = points;
            return this;
        }
    }

    public class PlayerSpawnEvent : GameEvent
    {
        public Vector2 spawnPos;

        public PlayerSpawnEvent Initializer(Vector2 spawnPos)
        {
            this.spawnPos = spawnPos;
            return this;
        }
    }

    public class StageSpawnEvent : GameEvent
    {
    }

    public class StageSelectEvent : GameEvent
    {
        public StageDataSO tempData;
        public bool isRight;

        public StageSelectEvent Initializer(bool isRight)
        {
            this.isRight = isRight;
            return this;
        }
    }

    public class StageChangeEvent : GameEvent
    {
        public int depth;

        public StageChangeEvent Initiailzier(int depth)
        {
            this.depth = depth;
            return this;
        }
    }

    public class StageInitEvent : GameEvent
    {
    }
}