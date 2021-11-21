using Verse.API.Models;

namespace UI {
    public class AddTileOperation : ITileOperation {
        private readonly Tile _tile;

        public AddTileOperation(Tile tile) {
            _tile = tile;
        }

        public void Execute() {
            _tile.Register();
        }

        public void Undo() {
            _tile.Unregister();
        }
    }
}