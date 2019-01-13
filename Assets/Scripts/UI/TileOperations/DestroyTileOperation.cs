using Verse.API.Models;

namespace UI {
    public class DestroyTileOperation : ITileOperation {
        private Tile _tile;

        public DestroyTileOperation(Tile tile) {
            _tile = tile;
        }

        public void Execute() {
            if (typeof(TileObjectEntity).IsInstanceOfType(_tile)) {
                ((TileObjectEntity) _tile).Destroy();
            }
            else if (typeof(TileObject).IsInstanceOfType(_tile)) {
                ((TileObject) _tile).Destroy();
            }
            else {
                _tile.Destroy();
            }
        }

        public void Undo() {
            _tile.Room.TileProvider.Add(_tile);
        }
    }
}