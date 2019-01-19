using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Verse.API.Models;
using Verse.API.Models.JSON;
using Verse.Utilities;

namespace Verse.Systems {
    //todo Cache each world with changes
    public static class WorldLoader {
        public static RoomColliders GetRoomColliders(string room) {
            var roomPath = GetPathToRoomFolder(room);
            var jsonString = File.ReadAllText(Path.Combine(roomPath, FileConstants.RoomDefinitionFileName));
            return JsonConvert.DeserializeObject<RoomColliders>(jsonString);
        }

        private static string GetPathToRoomFolder(string room) {
            foreach (var mod in ModMap.GetEnabledMods()) {
                for (int i = 0; i < mod.RoomNames.Length; i++) {
                    if (mod.RoomNames[i] == room) {
                        return mod.RoomPaths[i];
                    }
                }
            }

            return null;
        }

        public static List<Tile> GetTileMap(RoomOld roomOld) {
            var filePath = Path.Combine(GetPathToRoomFolder(roomOld.RoomName), FileConstants.RoomTileMapFileName);
            var jsonString = File.ReadAllText(filePath);
            var serializableTiles = JsonConvert.DeserializeObject<List<SerializableTile>>(jsonString);
            return serializableTiles.Select(sTile => sTile.ToTile(roomOld)).ToList();
        }

        public static List<TileObject> GetThingMap(RoomOld roomOld) {
            var filePath = Path.Combine(GetPathToRoomFolder(roomOld.RoomName), FileConstants.RoomTileObjectMapFileName);
            var jsonString = File.ReadAllText(filePath);
            var serializableThings = JsonConvert.DeserializeObject<List<SerializableTileObject>>(jsonString);

            return serializableThings.Select(sThing => sThing.ToTileObject(roomOld)).ToList();
        }

        public static List<TileObjectEntity> GetScriptableThings(RoomOld roomOld) {
            var filePath = Path.Combine(GetPathToRoomFolder(roomOld.RoomName),
                FileConstants.RoomTileObjectEntityMapFileName);
            var jsonString = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

            var sThings =
                JsonConvert.DeserializeObject<List<SerializableTileObjectEntity>>(jsonString, settings);

            return sThings.Select(sThing => sThing.ToScriptableTileObject(roomOld)).ToList();
        }
    }
}