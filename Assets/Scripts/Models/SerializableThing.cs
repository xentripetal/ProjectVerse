using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Mathematics;

public class SerializableThing {
    public string Definition;
    public float posX;
    public float posY;
    public IList<IThingData> Datasets;

    [JsonConstructor]
    public SerializableThing(string Definition, float posX, float posY, IList<IThingData> Datasets) {
        this.Definition = Definition;
        this.posX = posX;
        this.posY = posY;
        this.Datasets = Datasets;
    }

    public SerializableThing(Thing thing) {
        this.Definition = thing.Definition.Name;
        this.posX = thing.Position.x;
        this.posY = thing.Position.y;
        this.Datasets = thing.Datasets;
    }

    public Thing toThing() {
        return new Thing(this);
    }
}