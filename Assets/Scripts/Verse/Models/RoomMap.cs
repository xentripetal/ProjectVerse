using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Verse.API.Models.JSON;

namespace Verse.API.Models {
    public class RoomMap {
        private static Dictionary<string, Room> rooms;

        public static void VerifyAtlas() {
            if (rooms == null) {
                CreateAtlas();
            }
        }

        public static Room GetRoom(string roomName) {
            VerifyAtlas();

            return rooms[roomName];
        }

        public static Room[] GetRooms() {
            VerifyAtlas();
            return rooms.Values.ToArray();
        }

        private static void CreateAtlas() {
            rooms = new Dictionary<string, Room>();
            foreach (var mod in ModMap.GetEnabledMods()) {
                if (!mod.IsProvidingRooms) {
                    continue;
                }

                foreach (var path in mod.RoomPaths) {
                    var data = File.ReadAllText(path);
                    var sRoom = JsonConvert.DeserializeObject<SerializableRoom>(data);
                    var room = new Room(sRoom);
                    rooms.Add(room.Name, room);
                }
            }
        }
    }
}