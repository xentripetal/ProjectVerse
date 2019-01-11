using System;

namespace Verse.API.Interfaces.Events {
    [AttributeUsage(AttributeTargets.Method)]
    public class OnFrameUpdate : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class OnLateFrameUpdate : Attribute { }
}