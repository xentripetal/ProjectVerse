using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse {
    public class Mocks {
        public static Dictionary<string, Room> Rooms = new Dictionary<string, Room>() {
            {"roomA", new Room("roomA", "RoomA", "AutoRoom")},
            {"roomB", new Room("roomB", "RoomB", "AutoRoom")},
            {"roomC", new Room("roomC", "RoomC", "AutoRoom")},
            {"test", new Room("test", "Test", "Test")}
        };

        public static Room FromId(string id) {
            if (Rooms.ContainsKey(id)) {
                return Rooms[id];
            }

            throw new Exception("Room not found");
        }
    }
}