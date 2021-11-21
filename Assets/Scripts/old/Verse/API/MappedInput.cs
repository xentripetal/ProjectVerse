using System.Collections.Generic;
using UnityEngine;

namespace Verse.API {
    public static class MappedInput {
        private static readonly Dictionary<string, KeyCode> mappedKeys = new Dictionary<string, KeyCode>();

        public static void AddMapping(string mapName, KeyCode input) {
            mappedKeys.Add(mapName, input);
        }

        public static void ChangeMapping(string mapName, KeyCode input) {
            mappedKeys[mapName] = input;
        }

        public static void RemoveMapping(string mapName) {
            mappedKeys.Remove(mapName);
        }

        public static bool GetKeyDown(string mapName) {
            return Input.GetKeyDown(mappedKeys[mapName]);
        }

        public static bool GetKeyUp(string mapName) {
            return Input.GetKeyUp(mappedKeys[mapName]);
        }

        public static bool GetKey(string mapName) {
            return Input.GetKey(mappedKeys[mapName]);
        }
    }
}