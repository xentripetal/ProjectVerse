using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine.Tilemaps;

public class ThingConverter : JsonConverter {
    public override bool CanWrite => false;
    public override bool CanRead => true;

    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Thing[]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer) {
        if (reader.TokenType != JsonToken.StartObject)
            return null;
        
        reader.Read(); // Wrapper Object
        reader.Read(); // Property Name
        
        List<Thing> things = new List<Thing>();
        
        if (reader.TokenType == JsonToken.StartArray) {
            while (reader.TokenType != JsonToken.EndArray) {
                if (reader.TokenType == JsonToken.StartObject) {
                    Thing thing = readThing(reader);
                    things.Add(thing);
                }
            }
        }

        return things;
    }

    private Thing readThing(JsonReader reader) {
        string definition = "";
        float x = 0f;
        float y = 0f;

        if (reader.ReadAsString() == "name") {
            definition = reader.ReadAsString();
        }
        else {
            Debug.LogError("Object has no definition.");
            return null;
        }

        if (reader.ReadAsString() == "X") {
            x = (float) reader.ReadAsDouble();
        }
        else {
            Debug.LogError("Object " + definition + " has no Y coordinate.");
            return null;
        }

        if (reader.ReadAsString() == "Y") {
            y = (float) reader.ReadAsDouble();
            return null;
        }
        else {
            Debug.LogError("Object " + definition + " has no Y coordinate.");
        }

        IThingData[] datasets = new IThingData[]{};
        if (reader.TokenType == JsonToken.PropertyName && reader.ReadAsString() == "datasets") {
            datasets = getDatasets(definition, reader);
        }

        return null;
    }

    private IThingData[] getDatasets(string definition, JsonReader reader) {
        while (reader.TokenType != JsonToken.EndArray) {
            reader.Read(); //Start Object
            string dataClass = reader.ReadAsString(); //PropertyName
            object[] args = new object[] {};
            if (reader.TokenType == JsonToken.StartObject) {
                while (reader.TokenType != JsonToken.EndObject) {
                    
                }
            }
            else {
                Debug.LogError("Dataset " + dataClass + " for object " + definition + " has no data.");
            }
            Type dataType = ScriptAtlas.getScript(dataClass).DataModel;
            IThingData dataModel = (IThingData) Activator.CreateInstance(dataType, args);
        }
        return null;
    }
}
