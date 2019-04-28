using System;
using System.Collections.Generic;
using System.Reflection;
using Fasterflect;
using Verse.API.Models;

namespace Verse.API.Events.EventBus {
    public class SortedEventList {
        private readonly SortedList<int, IEventHandler> list;

        public SortedEventList() {
            list = new SortedList<int, IEventHandler>(new DuplicateKeyComparer<int>());
        }

        public void Invoke(object @event) {
            foreach (var action in list) action.Value.Invoke(@event);
        }

        public void Add(MethodInfo info, int priority) {
            var invoker = info.DelegateForCallMethod();
            list.Add(priority, new CachedEventHandler(invoker));
        }

        public void Add<T>(Action<T> action, int priority) {
            list.Add(priority, new ActionEventHandler<T>(action));
        }

        public bool Contains<T>(Action<T> action) {
            var compare = new ActionEventHandler<T>(action);
            return list.ContainsValue(compare);
        }

        public bool Remove<T>(Action<T> action) {
            var compare = new ActionEventHandler<T>(action);
            var pos = list.IndexOfValue(compare);
            if (pos == -1) return false;

            list.RemoveAt(pos);
            return true;
        }
    }
}