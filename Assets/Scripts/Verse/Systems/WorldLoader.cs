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

        public static List<Tile> GetTileMap(Room room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room.RoomName + "/TileMap").text;

            var serializableTiles = JsonConvert.DeserializeObject<List<SerializableTile>>(jsonString);

            return serializableTiles.Select(sTile => sTile.ToTile(room)).ToList();
        }

        public static List<TileObject> GetThingMap(Room room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room.RoomName + "/ObjectMap").text;
            var serializableThings = JsonConvert.DeserializeObject<List<SerializableTileObject>>(jsonString);

            return serializableThings.Select(sThing => sThing.ToTileObject(room)).ToList();
        }

        public static List<TileObjectEntity> GetScriptableThings(Room room) {
            var jsonString = Resources.Load<TextAsset>("Rooms/" + room.RoomName + "/ScriptableObjectMap").text;
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

            var sThings =
                JsonConvert.DeserializeObject<List<SerializableTileObjectEntity>>(jsonString, settings);

            return sThings.Select(sThing => sThing.ToScriptableTileObject(room)).ToList();
        }
    }
}