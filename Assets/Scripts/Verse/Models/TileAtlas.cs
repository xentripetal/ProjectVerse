using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Verse.API.Models {
    public class TileAtlas {
        private static Dictionary<String, TileDef> _tileMap;
        private static Dictionary<String, TileObjectDef> _tileObjectMap;
        private static Dictionary<String, TileObjectEntityDef> _scriptableTileObjectMap;

        public static void VerifyAtlas() {
            if (_tileMap == null) {
                CreateTileAtlas();
            }

            if (_tileObjectMap == null) {
                CreateTileObjectAtlas();
            }

            if (_scriptableTileObjectMap == null) {
                CreateScriptableTileObjectAtlas();
            }
        }

        public static void RegenerateAtlas() {
            CreateTileAtlas();
            CreateTileObjectAtlas();
            CreateScriptableTileObjectAtlas();
        }

        public static string[] GetKeys() {
            VerifyAtlas();
            var keys = GetTileNames().ToList();
            keys.AddRange(GetTileObjectNames());
            keys.AddRange(GetScriptableTileObjectNames());
            return keys.ToArray();
        }

        public static string[] GetTileNames() {
            VerifyAtlas();
            return _tileMap.Keys.ToArray();
        }

        public static string[] GetTileObjectNames() {
            VerifyAtlas();
            return _tileObjectMap.Keys.ToArray();
        }

        public static string[] GetScriptableTileObjectNames() {
            VerifyAtlas();
            return _scriptableTileObjectMap.Keys.ToArray();
        }

        public static TileDef GetDef(string objectName) {
            VerifyAtlas();
            if (_tileMap.ContainsKey(objectName)) {
                return GetTileDef(objectName);
            }
            else if (_tileObjectMap.ContainsKey(objectName)) {
                return GetTileObjectDef(objectName);
            }
            else if (_scriptableTileObjectMap.ContainsKey(objectName)) {
                return GetScriptableTileObjectDef(objectName);
            }

            Debug.LogError("Definition " + objectName + " does not exist");
            return null;
        }

        public static TileDef GetTileDef(String objectName) {
            VerifyAtlas();
            return _tileMap[objectName];
        }

        public static TileObjectDef GetTileObjectDef(String objectName) {
            VerifyAtlas();
            return _tileObjectMap[objectName];
        }

        public static TileObjectEntityDef GetScriptableTileObjectDef(String objectName) {
            VerifyAtlas();
            return _scriptableTileObjectMap[objectName];
        }

        private static void CreateTileAtlas() {
            _tileMap = new Dictionary<string, TileDef>();
            var tiles = Resources.LoadAll<TextAsset>("Defs/Tiles");
            foreach (var tile in tiles) {
                var def = JsonConvert.DeserializeObject<TileDef>(tile.text);
                _tileMap.Add(def.FullName, def);
            }
        }

        private static void CreateTileObjectAtlas() {
            _tileObjectMap = new Dictionary<string, TileObjectDef>();
            var tiles = Resources.LoadAll<TextAsset>("Defs/Things");
            foreach (var tile in tiles) {
                var def = JsonConvert.DeserializeObject<TileObjectDef>(tile.text);
                _tileObjectMap.Add(def.FullName, def);
            }
        }

        private static void CreateScriptableTileObjectAtlas() {
            _scriptableTileObjectMap = new Dictionary<string, TileObjectEntityDef>();
            var tiles = Resources.LoadAll<TextAsset>("Defs/ScriptableThings");
            foreach (var tile in tiles) {
                var def = JsonConvert.DeserializeObject<TileObjectEntityDef>(tile.text);
                _scriptableTileObjectMap.Add(def.FullName, def);
            }
        }
    }
}