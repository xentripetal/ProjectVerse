using System;

namespace Verse.API.Scripting {
    public interface IThingScript {
        Type DataModel { get; }
    }
}