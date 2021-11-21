using Fasterflect;

namespace Verse.API.Events.EventBus {
    public class CachedEventHandler : IEventHandler {
        private readonly object _caller;
        private readonly MethodInvoker _invoker;

        public CachedEventHandler(MethodInvoker invoker) {
            _invoker = invoker;
            _caller = null;
        }

        public CachedEventHandler(MethodInvoker invoker, object caller) {
            _invoker = invoker;
            _caller = null;
        }

        public void Invoke(object param) {
            _invoker.Invoke(_caller, param);
        }
    }
}