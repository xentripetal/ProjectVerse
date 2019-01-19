namespace Verse.API.Models {
    public abstract class RoomOld {
        public virtual bool IsRoomLoaded { get; protected set; }

        public virtual string RoomName { get; protected set; }
        public virtual string World { get; protected set; }

        public virtual RoomColliders RoomColliders { get; protected set; }

        public virtual TileProviderOld TileProviderOld { get; protected set; }
        public virtual ModPackage ModPackage { get; protected set; }
    }
}