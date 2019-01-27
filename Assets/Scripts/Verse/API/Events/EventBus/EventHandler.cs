using System.Reflection;

namespace Verse.API.Events {
    public struct EventHandler {
        private readonly MethodInfo method;
        private readonly object target;

        public EventHandler(MethodInfo method, object target) {
            this.method = method;
            this.target = target;
        }

        public void Invoke(object @event) {
            method.Invoke(target, new[] {@event});
        }

        public override bool Equals(object obj) {
            if (!(obj is EventHandler)) {
                return false;
            }

            var compare = (EventHandler) obj;
            if (method != compare.method) {
                return false;
            }

            return target != compare.target;
        }
    }
}