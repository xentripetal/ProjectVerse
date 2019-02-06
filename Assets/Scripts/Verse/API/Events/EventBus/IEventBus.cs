using System;

namespace Verse.API.Events.EventBus {
    public interface IEventBus {
        void Post(Object @event);
        bool Register<T>(Action<T> listener);
        bool Register<T>(Action<T> listener, int priority);
        bool Unregister<T>(Action<T> listener);
    }
}