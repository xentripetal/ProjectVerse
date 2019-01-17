using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Verse.API.Models {
    public class TileDefMap {
        private static Dictionary<String, TileDef> _tileMap;
        private static Dictionary<String, TileObjectDef> _tileObjectMap;
        private static Dictionary<String, TileObjectEntityDef> _tileObjectEntityMap;

        public static void Verify() {
            if (_tileMap == null) {
                CreateTileDefMap();
            }

            if (_tileObjectMap == null) {
                CreateTileObjectDefMap();
            }

            if (_tileObjectEntityMap == null) {
                CreateTileObjectEntityDefMap();
            }
        }

        public static void RegenerateAtlas() {
            CreateTileDefMap();
            CreateTileObjectDefMap();
            CreateTileObjectEntityDefMap();
        }

        public static string[] GetKeys() {
            Verify();
            var keys = GetTileNames().ToList();
            keys.AddRange(GetTileObjectNames());
            keys.AddRange(GetTileObjectEntityDefsNames());
            return keys.ToArray();
        }

        public static string[] GetTileNames() {
            Verify();
            return _tileMap.Keys.ToArray();
        }

        public static string[] GetTileObjectNames() {
            Verify();
            return _tileObjectMap.Keys.ToArray();
        }

        public static string[] GetTileObjectEntityDefsNames() {
            Verify();
            return _tileObjectEntityMap.Keys.ToArray();
        }

        public static TileDef GetDef(string objectName) {
            Verify();
            if (_tileMap.ContainsKey(objectName)) {
                return GetTileDef(objectName);
            }
            else if (_tileObjectMap.ContainsKey(objectName)) {
                return GetTileObjectDef(objectName);
            }
            else if (_tileObjectEntityMap.ContainsKey(objectName)) {
                return GetScriptableTileObjectDef(objectName);
            }

            Debug.LogError("Definition " + objectName + " does not exist");
            return null;
        }

        public static TileDef GetTileDef(String objectName) {
            Verify();
            return _tileMap[objectName];
        }

        public static TileObjectDef GetTileObjectDef(String objectName) {
            Verify();
            return _tileObjectMap[objectName];
        }

        public static TileObjectEntityDef GetScriptableTileObjectDef(String objectName) {
            Verify();
            return _tileObjectEntityMap[objectName];
        }

        private static void CreateTileDefMap() {
            _tileMap = new Dictionary<string, TileDef>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (mod.IsProvidingTileDefs) {
                    var filenames = Directory.GetFiles(mod.TileDefsPath, "*.json");
                    foreach (var filename in filenames) {
                        var jsonString = File.ReadAllText(filename);
                        var def = JsonConvert.DeserializeObject<TileDef>(jsonString);
                        _tileMap.Add(def.FullName, def);
                    }
                }
            }
        }

        private static void CreateTileObjectDefMap() {
            _tileObjectMap = new Dictionary<string, TileObjectDef>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (mod.IsProvidingTileObjectDefs) {
                    var filenames = Directory.GetFiles(mod.TileObjectDefsPath, "*.json");
                    foreach (var filename in filenames) {
                        var jsonString = File.ReadAllText(filename);
                        var def = JsonConvert.DeserializeObject<TileObjectDef>(jsonString);
                        _tileObjectMap.Add(def.FullName, def);
                    }
                }
            }
        }

        private static void CreateTileObjectEntityDefMap() {
            _tileObjectEntityMap = new Dictionary<string, TileObjectEntityDef>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (mod.IsProvidingTileObjectEntityDefs) {
                    var filenames = Directory.GetFiles(mod.TileObjectEntityDefsPath, "*.json");
                    foreach (var filename in filenames) {
                        var jsonString = File.ReadAllText(filename);
                        var def = JsonConvert.DeserializeObject<TileObjectEntityDef>(jsonString);
                        _tileObjectEntityMap.Add(def.FullName, def);
                    }
                }
            }
        }
    }
}