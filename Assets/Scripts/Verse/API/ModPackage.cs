namespace Verse.API {
    public class ModPackage {
        public string Author;
        public string ContentPath;
        public string[] ContributingAuthors;
        public string[] DLLPaths;
        public bool IsPackageIntegrityValid;
        public bool IsProvidingDefs;
        public bool IsProvidingDLLs;
        public bool IsProvidingRoomPatches;
        public bool IsProvidingRooms;
        public bool IsProvidingTileDefs;
        public bool IsProvidingTileObjectDefs;
        public bool IsProvidingTileObjectEntityDefs;
        public string Name;
        public string[] RoomNames;
        public string RoomPatchesFolderPath;
        public string[] RoomPatchNames;
        public string[] RoomPatchPaths;
        public string[] RoomPaths;
        public string RoomsFolderPath;
        public string TileDefsPath;
        public string TileObjectDefsPath;
        public string TileObjectEntityDefsPath;
        public string Version;
    }
}