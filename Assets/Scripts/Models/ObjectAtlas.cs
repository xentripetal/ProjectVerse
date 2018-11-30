using System;
using System.Collections.Generic;
using Verse.API.Models;

public class ObjectAtlas {
    private static Dictionary<String, TileDef> objectAtlas;

    public static void VerifyAtlas() {
        if (objectAtlas == null) {
            createAtlas();
        }
    }

    public static TileDef getTileDef(String objectName) {
        VerifyAtlas();
        return objectAtlas[objectName];
    }
    
    public static ThingDef getThingDef(String objectName) {
        VerifyAtlas();
        return (ThingDef) objectAtlas[objectName];
    }

    public static ScriptableThingDef getScriptableThingDef(String objectName) {
        VerifyAtlas();
        return (ScriptableThingDef) objectAtlas[objectName];
    }

    private static void createAtlas() {
        SpriteInfo barrelSpriteInfo = new SpriteInfo("Sprites/Objects/barrel", 32, new Position(0,0), null);
        ThingDef barrel = new ThingDef("core.static.barrel", barrelSpriteInfo, true);
        SpriteInfo treeSpriteInfo = new SpriteInfo("Sprites/Objects/mediumtree1", 32, new Position(0,0), null);
        ThingDef tree = new ThingDef("core.vegetation.tree", treeSpriteInfo, true);
        IThingScript[] scripts = new IThingScript[] {new DoorTrigger()};
        SpriteInfo triggerSpriteInfo = new SpriteInfo("Sprites/Terrain/Collision", 32, new Position(0,0), null);
        ScriptableThingDef trigger = new ScriptableThingDef("core.trigger", triggerSpriteInfo, true, true, scripts);
        SpriteInfo grassSpriteInfo = new SpriteInfo("Sprites/Terrain/grass4x4tile", 32, new Position(0,0), null);
        TileDef grass = new TileDef("core.grass", grassSpriteInfo);
        SpriteInfo grass2SpriteInfo = new SpriteInfo("Sprites/Terrain/grass2-4x4tile", 32, new Position(0,0), null);
        TileDef grass2 = new TileDef("core.grass2", grass2SpriteInfo);
        objectAtlas = new Dictionary<string, TileDef>();
        objectAtlas.Add(barrel.Name, barrel);
        objectAtlas.Add(tree.Name, tree);
        objectAtlas.Add(trigger.Name, trigger);
        objectAtlas.Add(grass.Name, trigger);
        objectAtlas.Add(grass2.Name, trigger);
    }
}