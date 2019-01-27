using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse.API.Events.EventBus;
using Verse.API.Models;
using Object = System.Object;

namespace Verse.API.Events {
    public class NewEventBus {
        private readonly int defaultPriority = 5000;

        private Dictionary<Type, SortedList<int, EventHandler>> eventHandlers;

        public NewEventBus() {
            eventHandlers = new Dictionary<Type, SortedList<int, EventHandler>>();
            RegisterStaticListeners();
        }

        private void RegisterStaticListeners() {
            var listeners = GetStaticListeners();
            foreach (var listener in listeners) {
                var eventType = listener.GetParameters()[0].ParameterType;
                var handler = new EventHandler(listener, null);
                if (!StoreEventHandler(handler, eventType)) {
                    Debug.LogError("Failed to store static event for " + listener.Name);
                }
            }
        }

        private List<MethodInfo> GetStaticListeners() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes().SelectMany(type => type.GetMethods())).Where(
                    method =>
                        method.GetCustomAttribute(typeof(Subscribe)) != null && method.IsStatic
                ).ToList();
        }

        private bool StoreEventHandler(EventHandler handler, Type eventType) {
            return StoreEventHandler(handler, eventType, defaultPriority);
        }

        private bool StoreEventHandler(EventHandler handler, Type eventType, int priority) {
            SortedList<int, EventHandler> eventList;
            if (!eventHandlers.TryGetValue(eventType, out eventList)) {
                eventList = new SortedList<int, EventHandler>(new DuplicateKeyComparer<int>());
                eventHandlers.Add(eventType, eventList);
            }

            if (eventList.ContainsValue(handler)) {
                return false;
            }

            eventList.Add(priority, handler);
            return true;
        }

        public bool Post(Object @event) {
            SortedList<int, EventHandler> eventList;
            if (!eventHandlers.TryGetValue(@event.GetType(), out eventList)) {
                return false;
            }

            bool hasListener = false;
            foreach (var listener in eventList) {
                hasListener = true;
                listener.Value.Invoke(@event);
            }

            return hasListener;
        }

        public bool Register<T>(Action<T> listener) {
            return Register(listener, defaultPriority);
        }

        public bool Register<T>(Action<T> listener, int priority) {
            var handler = new EventHandler(listener.Method, listener.Target);
            return StoreEventHandler(handler, typeof(T), priority);
        }

        public bool Unregister<T>(Action<T> listener) {
            var handler = new EventHandler(listener.Method, listener.Target);
            SortedList<int, EventHandler> eventList;
            if (!eventHandlers.TryGetValue(typeof(T), out eventList)) {
                return false;
            }

            if (!eventList.ContainsValue(handler)) {
                return false;
            }

            var index = eventList.IndexOfValue(handler);
            eventList.RemoveAt(index);
            return true;
        }
    }
}