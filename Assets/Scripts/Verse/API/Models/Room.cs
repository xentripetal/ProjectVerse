using Verse.API.Models.JSON;

namespace Verse.API.Models {
    public class Room {
        public string Name { get; protected set; }
        public RoomColliders Colliders { get; protected set; }
        public TileProvider Tiles { get; protected set; }

        public Room(SerializableRoom sRoom) {
            Name = sRoom.Name;
            Colliders = sRoom.Colliders;

            Tiles = new LoadedTileProvider(sRoom, this);
        }
    }
}