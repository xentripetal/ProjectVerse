using System;
using System.Collections.Generic;

public class ObjectAtlas {
    private static Dictionary<String, ThingDef> objectAtlas;

    public static void InitializeAtlas() {
        if (objectAtlas == null) {
            createAtlas();
        }
    }

    public static ThingDef getObject(String objectName) {
        InitializeAtlas();
        return objectAtlas[objectName];
    }

    private static void createAtlas() {
        ThingDef barrel = new ThingDef("core.barrel", "Sprites/Objects/barrel", false, null);
        IThingScript[] scripts = new IThingScript[] {new DoorTrigger()};
        ThingDef trigger = new ThingDef("core.trigger", "Sprites/Terrain/Collision", true, scripts);
        objectAtlas = new Dictionary<string, ThingDef>();
        objectAtlas.Add(barrel.Name, barrel);
        objectAtlas.Add(trigger.Name, trigger);
    }
    
}
