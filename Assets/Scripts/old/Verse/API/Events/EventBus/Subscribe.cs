using System;

namespace Verse.API.Events.EventBus {
    [AttributeUsage(AttributeTargets.Method)]
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