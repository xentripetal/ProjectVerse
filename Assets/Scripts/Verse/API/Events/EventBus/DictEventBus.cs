using System;
using System.Collections.Generic;
using Verse.API.Events.EventBus;

namespace Verse.API.Events {
    public class DictEventBus : IEventBus {
        private readonly int defaultPriority = 5000;

        private Dictionary<Type, SortedEventList> eventHandlers;

        public DictEventBus() {
            eventHandlers = new Dictionary<Type, SortedEventList>();
        }

        private bool StoreEventHandler<T>(Action<T> handler, Type eventType) {
            return StoreEventHandler(handler, eventType, CoreEventPriorities.DefaultPriority);
        }

        private bool StoreEventHandler<T>(Action<T> handler, Type eventType, int priority) {
            SortedEventList eventListGeneric;
            SortedEventList<T> eventList;
            if (!eventHandlers.TryGetValue(eventType, out eventListGeneric)) {
                eventList = new SortedEventList<T>();
                eventHandlers.Add(eventType, eventList);
            }
            else {
                eventList = (SortedEventList<T>) eventListGeneric;
            }

            if (eventList.Contains(handler)) {
                return false;
            }

            eventList.Add(handler, priority);
            return true;
        }

        public void Post(Object @event) {
            SortedEventList eventListGeneric;
            if (!eventHandlers.TryGetValue(@event.GetType(), out eventListGeneric)) {
                return;
            }

            eventListGeneric.Invoke(@event);
        }

        public bool Register<T>(Action<T> listener) {
            return StoreEventHandler(listener, typeof(T));
        }

        public bool Register<T>(Action<T> listener, int priority) {
            return StoreEventHandler(listener, typeof(T), priority);
        }

        public bool Unregister<T>(Action<T> listener) {
            SortedEventList eventListGeneric;
            if (!eventHandlers.TryGetValue(typeof(T), out eventListGeneric)) {
                return false;
            }

            var eventList = (SortedEventList<T>) eventListGeneric;
            return eventList.Remove(listener);
        }
    }
}