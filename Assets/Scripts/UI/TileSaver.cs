using UnityEngine;
using Verse.API;
using Verse.API.Models;

public static class RoomFileSaver {
    private static readonly string TilesFileName = "TileMap.json";
    private static readonly string TileObjectsFileName = "ObjectMap.json";
    private static readonly string TileObjectEntitiesFileName = "ScriptableObjectMap.json";
    private static readonly string RoomDefinitionFileName = "MapDefinition.json";

    public static ModPackage ModPackage = ModMap.GetEnabledMods()[0];

    public static void SaveRoom(Room roomOld) {
        if (!roomOld.IsRoomLoaded) {
            Debug.LogError("Room " + roomOld.Name + " needs to be loaded to be saved");
            return;
        }

        Debug.LogError("Not yet implemented");
        //var roomFolderPath = GetOrCreateRoomDirectory(roomOld.Name);
        //WriteRoomDefinitionFile(roomOld, roomFolderPath);
    }

    /**

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

    private static void WriteRoomDefinitionFile(Room roomOld, string path) {
        var jsonString = JsonConvert.SerializeObject(roomOld.Colliders);
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
    }**/
}