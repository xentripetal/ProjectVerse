using System;
using Verse.API.Models.JSON;

namespace Verse.API.Models {
    public class Room {
        public string Name { get; protected set; }
        public RoomColliders Colliders { get; protected set; }
        public TileProvider Tiles { get; protected set; }
        public bool IsRoomLoaded { get; protected set; }

        //TODO seperate SRoom from Room
        public Room(SerializableRoom sRoom) {
            IsRoomLoaded = true;
            Name = sRoom.Name;
            Colliders = sRoom.Colliders;
            Tiles = new LoadedTileProvider(this);
            foreach (var sLayer in sRoom.Layers) {
                var layer = (TileLayer) Activator.CreateInstance(sLayer.LayerType);
                foreach (var sTile in sLayer.Tiles) {
                    sTile.ToTile(this, layer).Register();
                }
            }
        }
    }
}