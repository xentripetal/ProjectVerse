using System;

namespace Verse.API.Events.EventBus {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Subscribe : Attribute { }
}