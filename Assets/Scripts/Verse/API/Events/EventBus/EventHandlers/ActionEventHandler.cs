using System;

namespace Verse.API.Events.EventBus {
    public class ActionEventHandler<T> : IEventHandler {
        public Action<T> action;

        public ActionEventHandler(Action<T> action) {
            this.action = action;
        }

        public void Invoke(object param) {
            action.Invoke((T) param);
        }

        public override bool Equals(object obj) {
            return obj is Action<T> compareAction && compareAction.Equals(action) ||
                   obj is ActionEventHandler<T> compareHandler && compareHandler.action.Equals(action);
        }
    }
}