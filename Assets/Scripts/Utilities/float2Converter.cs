using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

public class float2Converter : JsonConverter{
    
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(float2);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        float2 val = (float2) value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(val.x);
        writer.WritePropertyName("y");
        writer.WriteValue(val.y);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject item = JObject.Load(reader);
        float x = item["x"].Value<float>();
        float y = item["y"].Value<float>();
        return new float2(x, y);
    }

}
