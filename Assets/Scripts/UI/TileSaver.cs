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

    public static void SaveRoom(RoomOldActual roomOld) {
        if (!roomOld.IsRoomLoaded) {
            Debug.LogError("Room " + roomOld.RoomName + " needs to be loaded to be saved");
            return;
        }

        var roomFolderPath = GetOrCreateRoomDirectory(roomOld.RoomName);
        WriteRoomDefinitionFile(roomOld, roomFolderPath);
        WriteTilesFile((TileProviderOldInternal) roomOld.TileProviderOld, roomFolderPath);
        WriteTileObjectsFile((TileProviderOldInternal) roomOld.TileProviderOld, roomFolderPath);
        WriteTileObjectEntitiesFile((TileProviderOldInternal) roomOld.TileProviderOld, roomFolderPath);
    }

    private static void WriteTilesFile(TileProviderOldInternal providerOld, string path) {
        var jsonString =
            JsonConvert.SerializeObject(providerOld.GetTiles().Select(tile => (SerializableTile) tile).ToArray());
        File.WriteAllText(Path.Combine(path, TilesFileName), jsonString);
    }

    private static void WriteTileObjectsFile(TileProviderOldInternal providerOld, string path) {
        var jsonString =
            JsonConvert.SerializeObject(providerOld.GetTileObjects().Select(tile => (SerializableTileObject) tile)
                .ToArray());
        File.WriteAllText(Path.Combine(path, TileObjectsFileName), jsonString);
    }

    private static void WriteTileObjectEntitiesFile(TileProviderOldInternal providerOld, string path) {
        var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        var jsonString = JsonConvert.SerializeObject(
            providerOld.GetTileObjectEntities().Select(tile => (SerializableTileObjectEntity) tile).ToArray(), settings);
        File.WriteAllText(Path.Combine(path, TileObjectEntitiesFileName), jsonString);
    }

    private static void WriteRoomDefinitionFile(RoomOldActual roomOld, string path) {
        var jsonString = JsonConvert.SerializeObject(roomOld.RoomColliders);
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