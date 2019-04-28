using Verse.API.Models;

namespace Verse.API {
    public class TileEntity {
        public Tile Tile { get; protected set; }

        public TileEntity Clone() {
            return new TileEntity();
        }

        public void OnStart() {
        }

        public void OnRoomEntered() {
        }

        public void OnRoomLoaded() {
        }

        public void OnRoomUnloaded() {
        }

        public void OnCharacterHit() {
        }

        public void OnCharacterInteract() {
        }

        public void OnCharacterEnter() {
        }

        public void OnCharacterExit() {
        }

        public void OnFrameUpdate() {
        }

        public void OnTickUpdate() {
        }

        public void OnRareTickUpdate() {
        }
    }
}