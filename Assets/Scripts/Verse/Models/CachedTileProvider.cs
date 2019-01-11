using System;
using System.Collections.Generic;
using Verse.API.Models;

namespace Verse.API.Models {
    public sealed class CachedTileProvider : TileProvider {
        private List<List<Tile>> _tiles;
        private List<TileObject> _tileObjects;
        private List<ScriptableTileObject> _scriptableTileObjects;

        public CachedTileProvider(string roomName) {
            RoomName = roomName;
        }

        public CachedTileProvider(LoadedTileProvider provider) {
            RoomName = provider.RoomName;
        }

        private void LoadData() { }

        public override Tile GetTileAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override TileObject GetTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override ScriptableTileObject GetScriptableTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override void RemoveTile(Tile tile) {
            throw new NotImplementedException();
        }

        public override void RemoveTileAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override void RemoveTileObject(TileObject tile) {
            throw new NotImplementedException();
        }

        public override void RemoveTileObjectAt(TilePosition position) {
            throw new NotImplementedException();
        }

        public override void RemoveScriptableTileObject(ScriptableTileObject tile) {
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

        public override List<ScriptableTileObject> GetScriptableTileObjects() {
            throw new NotImplementedException();
        }
    }
}