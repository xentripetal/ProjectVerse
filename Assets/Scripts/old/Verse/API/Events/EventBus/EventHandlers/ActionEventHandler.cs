using System;

namespace Verse.API.Events.EventBus {
    public class ActionEventHandler<T> : IEventHandler {
        public readonly Action<T> Action;

        public ActionEventHandler(Action<T> action) {
            this.Action = action;
        }

        public void Invoke(object param) {
            Action.Invoke((T) param);
        }

        public override bool Equals(object obj) {
            return obj is Action<T> compareAction && compareAction.Equals(Action) ||
                   obj is ActionEventHandler<T> compareHandler && compareHandler.Action.Equals(Action);
        }

        protected bool Equals(ActionEventHandler<T> other) {
            return Equals(Action, other.Action);
        }

        public override int GetHashCode() {
            return (Action != null ? Action.GetHashCode() : 0);
        }
    }
}