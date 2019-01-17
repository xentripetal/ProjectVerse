using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API;
using Verse.API.Models;
using Verse.API.Models.JSON;

public static class RoomFileSaver {
    private static readonly string TilesFileName = "TileMap.json";
    private static readonly string TileObjectsFileName = "ObjectMap.json";
    private static readonly string TileObjectEntitiesFileName = "ScriptableObjectMap.json";
    private static readonly string RoomDefinitionFileName = "MapDefinition.json";

    public static ModPackage ModPackage = ModMap.GetEnabledMods()[0];

    public static void SaveRoom(RoomActual room) {
        if (!room.IsRoomLoaded) {
            Debug.LogError("Room " + room.RoomName + " needs to be loaded to be saved");
            return;
        }

        var roomFolderPath = GetOrCreateRoomDirectory(room.RoomName);
        WriteRoomDefinitionFile(room, roomFolderPath);
        WriteTilesFile((TileProviderInternal) room.TileProvider, roomFolderPath);
        WriteTileObjectsFile((TileProviderInternal) room.TileProvider, roomFolderPath);
        WriteTileObjectEntitiesFile((TileProviderInternal) room.TileProvider, roomFolderPath);
    }

    private static void WriteTilesFile(TileProviderInternal provider, string path) {
        var jsonString =
            JsonConvert.SerializeObject(provider.GetTiles().Select(tile => (SerializableTile) tile).ToArray());
        File.WriteAllText(Path.Combine(path, TilesFileName), jsonString);
    }

    private static void WriteTileObjectsFile(TileProviderInternal provider, string path) {
        var jsonString =
            JsonConvert.SerializeObject(provider.GetTileObjects().Select(tile => (SerializableTileObject) tile)
                .ToArray());
        File.WriteAllText(Path.Combine(path, TileObjectsFileName), jsonString);
    }

    private static void WriteTileObjectEntitiesFile(TileProviderInternal provider, string path) {
        var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        var jsonString = JsonConvert.SerializeObject(
            provider.GetTileObjectEntities().Select(tile => (SerializableTileObjectEntity) tile).ToArray(), settings);
        File.WriteAllText(Path.Combine(path, TileObjectEntitiesFileName), jsonString);
    }

    private static void WriteRoomDefinitionFile(RoomActual room, string path) {
        var jsonString = JsonConvert.SerializeObject(room.RoomColliders);
        File.WriteAllText(Path.Combine(path, RoomDefinitionFileName), jsonString);
    }

    private static string GetOrCreateRoomDirectory(string roomName) {
        for (int i = 0; i < ModPackage.RoomNames.Length; i++) {
            if (ModPackage.RoomNames[i] == roomName) {
                return ModPackage.RoomPaths[i];
            }
        }

        var roomFolderPath = Path.Combine(ModPackage.RoomsFolderPath, roomName);
        Directory.CreateDirectory(roomFolderPath);

        return roomFolderPath;
    }
}