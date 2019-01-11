using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API.Models;
using Verse.API.Models.JSON;

namespace Verse.Systems {
    //todo Cache each world with changes
    public static class WorldLoader {
        public static RoomColliders GetRoomColliders(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/MapDefinition").text;

            return JsonConvert.DeserializeObject<RoomColliders>(jsonString);
        }

        public static List<Tile> GetTileMap(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/TileMap").text;

            var serializableTiles = JsonConvert.DeserializeObject<List<SerializableTile>>(jsonString);
            return serializableTiles.Select(sTile => (Tile) sTile).ToList();
        }

        public static List<TileObject> GetThingMap(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/ObjectMap").text;
            var serializableThings = JsonConvert.DeserializeObject<List<SerializableThing>>(jsonString);
            return serializableThings.Select(sThing => (TileObject) sThing).ToList();
        }

        public static List<ScriptableTileObject> GetScriptableThings(string room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/ScriptableObjectMap").text;
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

            var sThings =
                JsonConvert.DeserializeObject<List<SerializableScriptableThing>>(jsonString, settings);
            return sThings.Select(sThing => (ScriptableTileObject) sThing).ToList();
        }
    }
}