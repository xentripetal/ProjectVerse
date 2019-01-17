using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API.Models.JSON;
using Verse.Utilities;

namespace Verse.API.Models {
    public static class ModMap {
        private static ModPackage[] mods;
        private static ModPackage[] enabledMods;
        private static ModPackage[] disabledMods;

        private static void VerifyMap() {
            if (mods == null) {
                RebuildMap();
            }
        }

        public static void RebuildMap() {
            if (!Directory.Exists(FileConstants.ModsFolder)) {
                mods = new ModPackage[0];
                enabledMods = new ModPackage[0];
                disabledMods = new ModPackage[0];
                return;
            }

            List<ModPackage> validMods = new List<ModPackage>();
            foreach (var directory in Directory.GetDirectories(FileConstants.ModsFolder)) {
                var modPackagePath = Path.Combine(directory, FileConstants.ModPackageFileName);
                if (!File.Exists(modPackagePath)) {
                    Debug.LogError("Mod " + directory + " does not contain a " + FileConstants.ModPackageFileName);
                    continue;
                }

                var content = File.ReadAllText(modPackagePath);
                var sModPackage = JsonConvert.DeserializeObject<SerializableModPackage>(content);
                validMods.Add(sModPackage.ToModPackage(directory));
            }

            mods = validMods.ToArray();
            enabledMods = mods;
            disabledMods = new ModPackage[0];
        }

        public static ModPackage[] GetEnabledMods() {
            VerifyMap();
            return enabledMods;
        }

        public static ModPackage[] GetDisabledMods() {
            VerifyMap();
            return disabledMods;
        }

        public static ModPackage[] GetAllAvailableMods() {
            VerifyMap();
            return mods;
        }
    }
}