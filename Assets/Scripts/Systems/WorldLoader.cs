using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API.Models;

namespace Systems {
    public class WorldLoader {
        public static TerrainMap GetTerrainMap(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/TerrainMap").text;
            
            TerrainMap terrainMap = JsonConvert.DeserializeObject<TerrainMap>(jsonString);
            
            if (terrainMap.Colliders.BoxColliders == null) {
                terrainMap.Colliders.BoxColliders = new List<BoxCollider>();
            }

            return terrainMap;
        }

        public static IList<Thing> GetThingMap(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/ObjectMap").text;
            var serializableThings = JsonConvert.DeserializeObject<List<SerializableThing>>(jsonString);
            return serializableThings.Select(sThing => (Thing) sThing).ToList();
        }

        public static IList<ScriptableThing> GetScriptableThings(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/ScriptableObjectMap").text;
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
            
            var serializableThings = JsonConvert.DeserializeObject<List<SerializableScriptableThing>>(jsonString, settings);
            return serializableThings.Select(sThing => (ScriptableThing) sThing).ToList();
        }
    }
}