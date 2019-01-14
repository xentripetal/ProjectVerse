namespace Verse.API.Models {
    public abstract class Room {
        public virtual bool IsRoomLoaded { get; protected set; }

        public virtual string RoomName { get; protected set; }
        public virtual string World { get; protected set; }

        public virtual RoomColliders RoomColliders { get; protected set; }

        public virtual TileProvider TileProvider { get; protected set; }
    }
}