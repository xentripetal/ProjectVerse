using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API.Models;

public class ObjectAtlas {
    private static Dictionary<String, TileDef> tileAtlas;
    private static Dictionary<String, ThingDef> thingAtlas;
    private static Dictionary<String, ScriptableThingDef> scriptableThingAtlas;

    public static void VerifyAtlas() {
        if (tileAtlas == null) {
            createTileAtlas();
        }

        if (thingAtlas == null) {
            createThingAtlas();
        }

        if (scriptableThingAtlas == null) {
            createScriptableThingAtlas();
        }
    }

    public static void RegenerateAtlas() {
        createTileAtlas();
        createThingAtlas();
        createScriptableThingAtlas();
    }

    public static TileDef getTileDef(String objectName) {
        VerifyAtlas();
        return tileAtlas[objectName];
    }

    public static ThingDef getThingDef(String objectName) {
        VerifyAtlas();
        return thingAtlas[objectName];
    }

    public static ScriptableThingDef getScriptableThingDef(String objectName) {
        VerifyAtlas();
        return scriptableThingAtlas[objectName];
    }

    private static void createTileAtlas() {
        tileAtlas = new Dictionary<string, TileDef>();
        var tiles = Resources.LoadAll<TextAsset>("Defs/Tiles");
        foreach (var tile in tiles) {
            var def = JsonConvert.DeserializeObject<TileDef>(tile.text);
            tileAtlas.Add(def.FullName, def);
        }
    }

    private static void createThingAtlas() {
        thingAtlas = new Dictionary<string, ThingDef>();
        var tiles = Resources.LoadAll<TextAsset>("Defs/Things");
        foreach (var tile in tiles) {
            var def = JsonConvert.DeserializeObject<ThingDef>(tile.text);
            thingAtlas.Add(def.FullName, def);
        }
    }

    private static void createScriptableThingAtlas() {
        scriptableThingAtlas = new Dictionary<string, ScriptableThingDef>();
        var tiles = Resources.LoadAll<TextAsset>("Defs/ScriptableThings");
        foreach (var tile in tiles) {
            var def = JsonConvert.DeserializeObject<ScriptableThingDef>(tile.text);
            scriptableThingAtlas.Add(def.FullName, def);
        }
    }

/*    private static void createAtlas() {
        SpriteInfo barrelSpriteInfo = new SpriteInfo("Sprites/Objects/barrel", 32, new Position(0, 0), null);
        ThingDef barrel = new ThingDef("core.static.barrel", barrelSpriteInfo, true);

        List<Position[]> colliderShape = new List<Position[]>();
        float PPU = 32;
        Position[] treeShape = new[] {
            new Position(.8f, .1f),
            new Position(1.7f, .2f),
            new Position(1.4f, .5f),
            new Position(.9f, .6f),
            new Position(.4f, .5f),
            new Position(.1f, .2f)
        };
        colliderShape.Add(treeShape);
        SpriteInfo treeSpriteInfo =
            new SpriteInfo("Sprites/Objects/mediumtree1", 32, new Position(0.2578125f, 0), colliderShape);
        ThingDef tree = new ThingDef("core.vegetation.tree", treeSpriteInfo, true);

        string[] scripts = new[] {"DoorTrigger"};
        SpriteInfo triggerSpriteInfo = new SpriteInfo("Sprites/Terrain/Collision", 32, new Position(0, 0), null);
        ScriptableThingDef trigger = new ScriptableThingDef("core.trigger", triggerSpriteInfo, true, true, scripts);

        SpriteInfo grassSpriteInfo = new SpriteInfo("Sprites/Terrain/grass4x4tile", 32, new Position(0, 0), null);
        TileDef grass = new TileDef("core.grass", grassSpriteInfo);

        SpriteInfo grass2SpriteInfo = new SpriteInfo("Sprites/Terrain/grass2-4x4tile", 32, new Position(0, 0), null);
        TileDef grass2 = new TileDef("core.grass2", grass2SpriteInfo);

        objectAtlas = new Dictionary<string, TileDef>();

        Debug.Log(JsonConvert.SerializeObject(barrel));

        objectAtlas.Add(barrel.FullName, barrel);
        objectAtlas.Add(tree.FullName, tree);
        objectAtlas.Add(trigger.FullName, trigger);
        objectAtlas.Add(grass.FullName, grass);
        objectAtlas.Add(grass2.FullName, grass2);
    }*/
}