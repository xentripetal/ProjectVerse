using UnityEngine;
using Verse.Systems;

namespace Verse.API.Models {
    public sealed class RoomOldActual : RoomOld {
        public override TileProviderOld TileProviderOld { get; protected set; }
        public override string RoomName { get; protected set; }
        public override bool IsRoomLoaded { get; protected set; }
        public override RoomColliders RoomColliders { get; protected set; }

        public override ModPackage ModPackage { get; protected set; }

        public RoomOldActual(string roomName, ModPackage package) {
            RoomName = roomName;
            IsRoomLoaded = true;
            RoomColliders = WorldLoader.GetRoomColliders(RoomName);
            TileProviderOld = new LoadedTileProviderOld(this);
            ModPackage = package;
        }

        public void LoadRoom() {
            if (IsRoomLoaded) {
                Debug.Log("LoadRoom called on already loaded room " + RoomName);
                return;
            }

            TileProviderOld = new LoadedTileProviderOld((CachedTileProviderOld) TileProviderOld);
            IsRoomLoaded = true;
        }

        public void UnloadRoom() {
            if (!IsRoomLoaded) {
                Debug.Log("UnloadRoom called on an unloaded room " + RoomName);
                return;
            }

            TileProviderOld = new CachedTileProviderOld((LoadedTileProviderOld) TileProviderOld);
            IsRoomLoaded = false;
        }
    }
}