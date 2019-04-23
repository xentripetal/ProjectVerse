using System;

namespace Verse.API.Events.EventBus {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Subscribe : Attribute {
        public int priority;

        public Subscribe() {
            priority = CoreEventPriorities.DefaultPriority;
        }

        public Subscribe(int priority) {
            this.priority = priority;
        }
    }
}