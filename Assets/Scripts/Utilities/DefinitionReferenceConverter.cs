using System;
using Newtonsoft.Json;

public class DefinitionReferenceConverter : JsonConverter{
    
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(ThingDef);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        writer.WriteValue(((ThingDef) value).Name);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.ReadAsString() == "Definition") {
            string name = reader.ReadAsString();
            return ObjectAtlas.getObject(name);
        }

        return null;
    }

}
