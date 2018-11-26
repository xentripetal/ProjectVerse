using System;
using Newtonsoft.Json;

public class ThingConverter : JsonConverter {
    public override bool CanWrite => false;
    public override bool CanRead => true;

    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Thing[]);
    }
}
