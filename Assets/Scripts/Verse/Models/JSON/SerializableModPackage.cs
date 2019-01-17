using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse.Utilities;

namespace Verse.API.Models.JSON {
    public class SerializableModPackage {
        public string Name;
        public string Version;
        public string Author;
        public string[] ContributingAuthors;
        public string[] DLLs;

        public ModPackage ToModPackage(string folderPath) {
            var mod = BuildDefaults(folderPath);
            mod = AssignDLLProperties(mod);
            mod = AssignTileDefProperties(mod);
            mod = AssignRoomProperties(mod);
            mod = AssignRoomPatchPropertues(mod);
            mod = AssignPackageValidity(mod);
            return mod;
        }

        // Not yet implemented. 
        // TODO: Calculate folder checksums and compare to expected checksum
        private ModPackage AssignPackageValidity(ModPackage mod) {
            mod.IsPackageIntegrityValid = true;
            return mod;
        }

        // Not yet implemented
        private ModPackage AssignRoomPatchPropertues(ModPackage mod) {
            mod.IsProvidingRoomPatches = false;
            mod.RoomPatchesFolderPath = String.Empty;
            mod.RoomPatchPaths = new string[0];
            mod.RoomPatchNames = new string[0];
            return mod;
        }

        private ModPackage AssignRoomProperties(ModPackage mod) {
            mod.RoomsFolderPath = Path.Combine(mod.ContentPath, FileConstants.RoomsFolderName);
            if (!Directory.Exists(mod.RoomsFolderPath)) {
                mod.IsProvidingRooms = false;
                mod.RoomPaths = new string[0];
            }

            var validRooms = new List<string>();
            foreach (var roomFolder in Directory.GetDirectories(mod.RoomsFolderPath)) {
                if (File.Exists(Path.Combine(roomFolder, FileConstants.RoomDefinitionFileName))) {
                    validRooms.Add(roomFolder);
                }
                else {
                    Debug.LogError("Mod " + Name + " provides room folder " + roomFolder + " but no " +
                                   FileConstants.RoomDefinitionFileName + " exists");
                }
            }

            if (validRooms.Count == 0) {
                mod.IsProvidingRooms = false;
                mod.RoomPaths = new string[0];
                return mod;
            }

            mod.IsProvidingRooms = true;
            mod.RoomPaths = validRooms.ToArray();
            mod.RoomNames = validRooms.Select(Path.GetFileNameWithoutExtension).ToArray();
            return mod;
        }

        private ModPackage AssignTileDefProperties(ModPackage mod) {
            mod.TileDefsPath = Path.Combine(mod.ContentPath, FileConstants.TileDefsFolder);
            mod.TileObjectDefsPath = Path.Combine(mod.ContentPath, FileConstants.TileObjectDefsFolder);
            mod.TileObjectEntityDefsPath = Path.Combine(mod.ContentPath, FileConstants.TileObjectEntitiesDefsFolder);
            mod.IsProvidingTileDefs = DirectoryExistsAndHasFiles(mod.TileDefsPath, "*.json");
            mod.IsProvidingTileObjectDefs = DirectoryExistsAndHasFiles(mod.TileObjectDefsPath, "*.json");
            mod.IsProvidingTileObjectEntityDefs = DirectoryExistsAndHasFiles(mod.TileObjectEntityDefsPath, "*.json");
            mod.IsProvidingDefs = (mod.IsProvidingTileDefs || mod.IsProvidingTileObjectDefs ||
                                   mod.IsProvidingTileObjectEntityDefs);

            return mod;
        }

        private bool DirectoryExistsAndHasFiles(string directory, string searchPattern) {
            if (!Directory.Exists(directory)) {
                return false;
            }

            return Directory.GetFiles(directory, searchPattern).Length > 0;
        }

        private ModPackage AssignDLLProperties(ModPackage mod) {
            if (!(DLLs != null && DLLs.Length != 0)) {
                mod.IsProvidingDLLs = false;
                mod.DLLPaths = new string[0];
                return mod;
            }

            var validDLLPaths = new List<string>();
            foreach (var dllName in DLLs) {
                var path = Path.Combine(mod.ContentPath, dllName);
                if (File.Exists(path)) {
                    validDLLPaths.Add(path);
                }
                else {
                    Debug.LogError("Mod " + Name + " is attempting to register DLL " + dllName +
                                   " but no such dll exists in its directory.");
                }
            }

            if (validDLLPaths.Count == 0) {
                mod.IsProvidingDLLs = false;
                mod.DLLPaths = new string[0];
                return mod;
            }

            mod.IsProvidingDLLs = true;
            mod.DLLPaths = validDLLPaths.ToArray();
            return mod;
        }

        private ModPackage BuildDefaults(string folderPath) {
            var mod = new ModPackage();
            mod.Name = Name;
            mod.Version = Version;
            mod.Author = Author;
            mod.ContributingAuthors = ContributingAuthors;
            mod.ContentPath = folderPath;
            return mod;
        }
    }
}