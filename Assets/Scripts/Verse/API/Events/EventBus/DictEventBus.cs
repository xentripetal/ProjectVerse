using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse.API.Events.EventBus;

namespace Verse.API.Events {
    public class DictEventBus : IEventBus {
        private readonly int defaultPriority = 5000;

        private readonly Dictionary<Type, SortedEventList> eventHandlers;

        public DictEventBus() {
            eventHandlers = new Dictionary<Type, SortedEventList>();
        }

        public void Post(object @event) {
            SortedEventList eventListGeneric;
            if (!eventHandlers.TryGetValue(@event.GetType(), out eventListGeneric)) return;

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
            if (!eventHandlers.TryGetValue(typeof(T), out eventListGeneric)) return false;

            var eventList = eventListGeneric;
            return eventList.Remove(listener);
        }

        private bool StoreEventHandler<T>(Action<T> handler, Type eventType,
            int priority = CoreEventPriorities.DefaultPriority) {
            var eventList = GetOrCreateEventList(eventType);

            if (eventList.Contains(handler)) return false;

            eventList.Add(handler, priority);
            return true;
        }

        private SortedEventList GetOrCreateEventList(Type eventType) {
            SortedEventList eventList;
            if (!eventHandlers.TryGetValue(eventType, out eventList)) {
                eventList = new SortedEventList();
                eventHandlers.Add(eventType, eventList);
            }

            return eventList;
        }

        public bool Register(MethodInfo info, int priority) {
            var parameters = info.GetParameters();
            if (parameters.Length != 1) return false;
            var type = parameters[0].ParameterType;
            Debug.Log(type.FullName);
            var eventList = GetOrCreateEventList(type);
            eventList.Add(info, priority);
            Debug.Log($"Registered event {info.Name} with priority {priority}");
            return true;
        }
    }
}