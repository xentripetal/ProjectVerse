using Verse.API.Models;

namespace UI {
    public class DestroyTileOperation : ITileOperation {
        private Tile _tile;

        public DestroyTileOperation(Tile tile) {
            _tile = tile;
        }

        public void Execute() {
            _tile.Unregister();
        }

        public void Undo() {
            _tile.Register();
        }
    }
}