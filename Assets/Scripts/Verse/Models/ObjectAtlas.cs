using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API.Models;

namespace Verse.API.Models {
    public class ObjectAtlas {
        private static Dictionary<String, TileDef> _tileAtlas;
        private static Dictionary<String, TileObjectDef> _thingAtlas;
        private static Dictionary<String, ScriptableTileObjectDef> _scriptableThingAtlas;

        public static void VerifyAtlas() {
            if (_tileAtlas == null) {
                CreateTileAtlas();
            }

            if (_thingAtlas == null) {
                CreateThingAtlas();
            }

            if (_scriptableThingAtlas == null) {
                CreateScriptableThingAtlas();
            }
        }

        public static void RegenerateAtlas() {
            CreateTileAtlas();
            CreateThingAtlas();
            CreateScriptableThingAtlas();
        }

        public static string[] GetKeys() {
            var keys = GetTileNames().ToList();
            keys.AddRange(GetThingNames());
            keys.AddRange(GetScriptableThingNames());
            return keys.ToArray();
        }

        public static string[] GetTileNames() {
            return _tileAtlas.Keys.ToArray();
        }

        public static string[] GetThingNames() {
            return _thingAtlas.Keys.ToArray();
        }

        public static string[] GetScriptableThingNames() {
            return _scriptableThingAtlas.Keys.ToArray();
        }

        public static TileDef GetDef(string objectName) {
            VerifyAtlas();
            if (_tileAtlas.ContainsKey(objectName)) {
                return GetTileDef(objectName);
            }
            else if (_thingAtlas.ContainsKey(objectName)) {
                return GetThingDef(objectName);
            }
            else if (_scriptableThingAtlas.ContainsKey(objectName)) {
                return GetScriptableThingDef(objectName);
            }

            Debug.LogError("Definition " + objectName + " does not exist");
            return null;
        }

        public static TileDef GetTileDef(String objectName) {
            VerifyAtlas();
            return _tileAtlas[objectName];
        }

        public static TileObjectDef GetThingDef(String objectName) {
            VerifyAtlas();
            return _thingAtlas[objectName];
        }

        public static ScriptableTileObjectDef GetScriptableThingDef(String objectName) {
            VerifyAtlas();
            return _scriptableThingAtlas[objectName];
        }

        private static void CreateTileAtlas() {
            _tileAtlas = new Dictionary<string, TileDef>();
            var tiles = Resources.LoadAll<TextAsset>("Defs/Tiles");
            foreach (var tile in tiles) {
                var def = JsonConvert.DeserializeObject<TileDef>(tile.text);
                _tileAtlas.Add(def.FullName, def);
            }
        }

        private static void CreateThingAtlas() {
            _thingAtlas = new Dictionary<string, TileObjectDef>();
            var tiles = Resources.LoadAll<TextAsset>("Defs/Things");
            foreach (var tile in tiles) {
                var def = JsonConvert.DeserializeObject<TileObjectDef>(tile.text);
                _thingAtlas.Add(def.FullName, def);
            }
        }

        private static void CreateScriptableThingAtlas() {
            _scriptableThingAtlas = new Dictionary<string, ScriptableTileObjectDef>();
            var tiles = Resources.LoadAll<TextAsset>("Defs/ScriptableThings");
            foreach (var tile in tiles) {
                var def = JsonConvert.DeserializeObject<ScriptableTileObjectDef>(tile.text);
                _scriptableThingAtlas.Add(def.FullName, def);
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
}