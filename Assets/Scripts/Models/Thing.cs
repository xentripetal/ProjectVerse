using Unity.Mathematics;

public class Thing {
    public ThingDef definition { get; private set; }
    public float2 position { get; private set; }
    public IThingData[] datasets { get; private set; }

    public Thing(ThingDef definition, float2 position, IThingData[] datasets) {
        this.definition = definition;
        this.position = position;
        this.datasets = datasets;
    }

}
