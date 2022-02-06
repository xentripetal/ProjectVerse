using System;

namespace Verse {
    public class Room {
        public Room(string id, string name, string sceneName) {
            Id = id;
            Name = name;
            SceneName = sceneName;
        }

        public string Name { get; protected set; }

        public String Id { get; protected set; }

        public String SceneName { get; protected set; }
    }
}