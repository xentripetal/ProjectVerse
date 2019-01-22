using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Verse.API.Models.JSON;

namespace Verse.API.Models {
    public class TileDefMap {
        private static Dictionary<String, TileDef> _tileMap;

        public static void Verify() {
            if (_tileMap == null) {
                RegenerateAtlas();
            }
        }

        public static void RegenerateAtlas() {
            _tileMap = new Dictionary<string, TileDef>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (mod.IsProvidingTileDefs) {
                    var filenames = Directory.GetFiles(mod.TileDefsPath, "*.json");
                    foreach (var filename in filenames) {
                        var jsonString = File.ReadAllText(filename);
                        var def = JsonConvert.DeserializeObject<SerializableTileDef>(jsonString);
                        _tileMap.Add(def.Name, def.ToTileUnifiedDef(mod));
                    }
                }
            }
        }

        public static List<String> GetKeys() {
            Verify();
            return _tileMap.Keys.ToList();
        }

        public static TileDef GetTileDef(String objectName) {
            Verify();
            return _tileMap[objectName];
        }
    }
}