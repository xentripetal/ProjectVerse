namespace Verse.API {
    public class ModPackage {
        public string Name;
        public string Version;
        public string Author;
        public string[] ContributingAuthors;
        public string ContentPath;
        public string[] DLLPaths;
        public string TileDefsPath;
        public string TileObjectDefsPath;
        public string TileObjectEntityDefsPath;
        public string RoomsFolderPath;
        public string RoomPatchesFolderPath;
        public string[] RoomPaths;
        public string[] RoomNames;
        public string[] RoomPatchPaths;
        public string[] RoomPatchNames;
        public bool IsPackageIntegrityValid;
        public bool IsProvidingDLLs;
        public bool IsProvidingDefs;
        public bool IsProvidingTileDefs;
        public bool IsProvidingTileObjectDefs;
        public bool IsProvidingTileObjectEntityDefs;
        public bool IsProvidingRooms;
        public bool IsProvidingRoomPatches;
    }
}