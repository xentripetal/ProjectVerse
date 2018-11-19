using Unity.Mathematics;

public class Thing {
    public ThingDef definition { get; private set; }
    public float2 position { get; private set; }
    public ThingScriptData[] datasets { get; private set; }

    public Thing(ThingDef definition, float2 position, ThingScriptData[] datasets) {
        this.definition = definition;
        this.position = position;
        //TODO Load defaults
        this.datasets = datasets;
    }

}
