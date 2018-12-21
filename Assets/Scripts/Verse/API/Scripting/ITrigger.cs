namespace Verse.API.Scripting {
    public interface ITrigger : IThingScript {
        void OnPlayerEnter(Player player, IThingData data);
    }
}