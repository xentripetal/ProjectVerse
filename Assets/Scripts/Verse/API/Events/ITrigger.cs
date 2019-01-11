namespace Verse.API.Interfaces.Events {
    public interface ITrigger : IThingScript {
        void OnPlayerEnter(IThingData data);
    }
}