using System;
using Fasterflect;

namespace Verse.API.Events.EventBus {
    public class CachedEventHandler : IEventHandler {
        private MethodInvoker _invoker;
        private object _caller;
        
        public CachedEventHandler(MethodInvoker invoker) {
            _invoker = invoker;
            _caller = null;
        }

        public CachedEventHandler(MethodInvoker invoker, Object caller) {
            _invoker = invoker;
            _caller = null;
        }
        
        public void Invoke(object param) {
            _invoker.Invoke(_caller, param);
        }
    }
}