using System.Collections.Generic;
using System.Linq;

namespace Verse.API.Models {
    public static class RoomAtlasOld {
        private static Dictionary<string, RoomOld> rooms;

        public static void InitializeAtlas() {
            if (rooms == null) {
                CreateAtlas();
            }
        }

        public static RoomOld GetRoom(string roomName) {
            InitializeAtlas();
            return rooms[roomName];
        }

        public static RoomOld[] GetRooms() {
            InitializeAtlas();
            return rooms.Values.ToArray();
        }

        private static void CreateAtlas() {
            rooms = new Dictionary<string, RoomOld>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (!mod.IsProvidingRooms) {
                    continue;
                }

                foreach (var roomName in mod.RoomNames) {
                    var room = new RoomOldActual(roomName, mod);
                    rooms.Add(roomName, room);
                }
            }
        }
    }
}