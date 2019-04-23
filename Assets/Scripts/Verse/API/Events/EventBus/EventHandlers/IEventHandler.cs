namespace Verse.API.Events.EventBus {
    public interface IEventHandler {
        void Invoke(object param);
    }
}