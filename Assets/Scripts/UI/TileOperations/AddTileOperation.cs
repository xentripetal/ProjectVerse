using Verse.API.Models;

namespace UI {
    public class AddTileOperation : ITileOperation {
        private Tile _tile;

        public AddTileOperation(Tile tile) {
            _tile = tile;
        }

        public void Execute() {
            _tile.RoomOld.TileProviderOld.Add(_tile);
        }

        public void Undo() {
            _tile.Destroy();
        }
    }
}