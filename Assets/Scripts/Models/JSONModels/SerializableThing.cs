using System.Collections.Generic;
using API.Models;
using Newtonsoft.Json;
using Unity.Mathematics;

public class SerializableThing {
    public string Definition;
    public Position Position;
    public IList<IThingData> Datasets;

    [JsonConstructor]
    public SerializableThing(string definition, Position position, IList<IThingData> datasets) {
        Definition = definition;
        Position = position;
        Datasets = datasets;
    }

    public SerializableThing(Thing thing) {
        this.Definition = thing.Definition.Name;
        this.Position = thing.Position;
        this.Datasets = thing.Datasets;
    }

    public Thing toThing() {
        return new Thing(this);
    }
}