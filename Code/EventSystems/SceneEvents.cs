using HN.Code.ETC.Scene;

namespace HN.Code.EventSystems
{
    public static class SceneEvents
    {
        public static readonly SceneChangeEvent SceneChangeEvent = new SceneChangeEvent();
    }

    public class SceneChangeEvent : GameEvent
    {
        public SceneDataSO sceneData;

        public SceneChangeEvent Initializer(SceneDataSO sceneData)
        {
            this.sceneData = sceneData;
            return this;
        }
    }
}