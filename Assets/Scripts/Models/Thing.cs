using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Mathematics;

public class Thing {
    public ThingDef Definition { get; }
    public float2 Position { get; }
    public IList<IThingData> Datasets { get; }

    public Thing(ThingDef Definition, float2 Position, IList<IThingData> Datasets) {
        this.Definition = Definition;
        this.Position = Position;
        this.Datasets = Datasets;
    }

    public Thing(SerializableThing serializableThing) {
        Definition = ObjectAtlas.getObject(serializableThing.Definition);
        Position = serializableThing.Position;
        Datasets = serializableThing.Datasets;
    }

    public override string ToString() {
        var concatstring = "{Definition: " + Definition.Name + ", Position: " + Position + ", Datasets: {";
        foreach (var dataset in Datasets) {
            concatstring += dataset.GetType().Name + ",";
        }

        concatstring += "}";
        return concatstring;
    }
}