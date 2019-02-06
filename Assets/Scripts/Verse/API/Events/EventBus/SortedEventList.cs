using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.API.Models;
using Object = System.Object;

namespace Verse.API.Events.EventBus {
    public abstract class SortedEventList {
        public abstract void Invoke(Object @event);
        public abstract Type Type { get; protected set; }
    }

    public class SortedEventList<T> : SortedEventList {
        private SortedList<int, Action<T>> list;

        public SortedEventList() {
            list = new SortedList<int, Action<T>>(new DuplicateKeyComparer<int>());
            Type = typeof(T);
        }

        public override void Invoke(Object @event) {
            if (!(@event is T)) {
                Debug.LogError("Event called for " + typeof(T).FullName + " has wrong type");
                return;
            }

            foreach (var action in list) {
                action.Value.Invoke((T) @event);
            }
        }

        public override Type Type { get; protected set; }

        public void Add(Action<T> action, int priority) {
            list.Add(priority, action);
        }

        public bool Contains(Action<T> action) {
            return list.ContainsValue(action);
        }

        public bool Remove(Action<T> action) {
            var pos = list.IndexOfValue(action);
            if (pos == -1) {
                return false;
            }

            return RemoveAt(pos);
        }

        public bool RemoveAt(int position) {
            if (position < 0 || position > list.Count) {
                return false;
            }

            list.RemoveAt(position);
            return true;
        }
    }
}