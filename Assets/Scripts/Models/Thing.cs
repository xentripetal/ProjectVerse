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
        Position = new float2(serializableThing.posX, serializableThing.posY);
        Datasets = serializableThing.Datasets;
    }
}