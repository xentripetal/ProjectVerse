using System.Collections.Generic;

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

        private static void CreateAtlas() {
            rooms = new Dictionary<string, Room>();
            rooms.Add("main", new RoomActual("main"));
            rooms.Add("test", new RoomActual("test"));
        }
    }
}