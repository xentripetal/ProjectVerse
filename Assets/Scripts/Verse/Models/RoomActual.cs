using UnityEngine;
using Verse.Systems;

namespace Verse.API.Models {
    public sealed class RoomActual : Room {
        public override TileProvider TileProvider { get; protected set; }
        public override string RoomName { get; protected set; }
        public override bool IsRoomLoaded { get; protected set; }
        public override RoomColliders RoomColliders { get; protected set; }

        public RoomActual(string roomName) {
            RoomName = roomName;
            TileProvider = new LoadedTileProvider(RoomName);
            IsRoomLoaded = true;
            RoomColliders = WorldLoader.GetRoomColliders(RoomName);
        }

        public void LoadRoom() {
            if (IsRoomLoaded) {
                Debug.Log("LoadRoom called on already loaded room " + RoomName);
                return;
            }

            TileProvider = new LoadedTileProvider((CachedTileProvider) TileProvider);
            IsRoomLoaded = true;
        }

        public void UnloadRoom() {
            if (!IsRoomLoaded) {
                Debug.Log("UnloadRoom called on an unloaded room " + RoomName);
                return;
            }

            TileProvider = new CachedTileProvider((LoadedTileProvider) TileProvider);
            IsRoomLoaded = false;
        }
    }
}