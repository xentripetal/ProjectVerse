namespace Verse.API.Interfaces {
    public interface ITrigger : IThingScript {
        void OnPlayerEnter(IPlayerReadOnly player, IThingData data);
    }
}