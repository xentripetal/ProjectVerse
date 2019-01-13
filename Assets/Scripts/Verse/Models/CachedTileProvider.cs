using System;
using System.Collections.Generic;

namespace Verse.API.Models {
    public sealed class CachedTileProvider : TileProviderInternal {
        private List<List<Tile>> _tiles;
        private List<TileObject> _tileObjects;
        private List<TileObjectEntity> _scriptableTileObjects;

        public CachedTileProvider(string roomName) { }

        public CachedTileProvider(LoadedTileProvider provider) { }

        private void LoadData() { }

        public override void Add(Tile tile) {
            throw new NotImplementedException();
        }

        public override Tile GetTileAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override TileObject GetTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override TileObjectEntity GetScriptableTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override void Remove(Tile tile) {
            throw new NotImplementedException();
        }

        public override void RemoveTileAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override void RemoveTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override void RemoveScriptableTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override bool TileOccupied(TilePosition position) {
            throw new NotImplementedException();
        }

        public override bool TileObjectOccupied(TilePosition position) {
            throw new NotImplementedException();
        }

        public override bool ScriptableTileObjectOccupied(TilePosition position) {
            throw new NotImplementedException();
        }

        public override List<Tile> GetTiles() {
            throw new NotImplementedException();
        }

        public override List<TileObject> GetTileObjects() {
            throw new NotImplementedException();
        }

        public override List<TileObjectEntity> GetScriptableTileObjects() {
            throw new NotImplementedException();
        }
    }
}