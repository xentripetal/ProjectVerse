using System.Collections.Generic;
using System.Linq;

namespace Verse.API.Models {
    public static class RoomAtlas {
        private static Dictionary<string, Room> rooms;

        public static void InitializeAtlas() {
            if (rooms == null) {
                CreateAtlas();
            }
        }

        public static Room GetRoom(string roomName) {
            InitializeAtlas();
            return rooms[roomName];
        }

        public static Room[] GetRooms() {
            InitializeAtlas();
            return rooms.Values.ToArray();
        }

        private static void CreateAtlas() {
            rooms = new Dictionary<string, Room>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (!mod.IsProvidingRooms) {
                    continue;
                }

                foreach (var roomName in mod.RoomNames) {
                    var room = new RoomActual(roomName, mod);
                    rooms.Add(roomName, room);
                }
            }
        }
    }
}