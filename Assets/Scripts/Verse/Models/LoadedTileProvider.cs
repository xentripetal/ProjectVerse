using System.Collections.Generic;
using Verse.API.Models;
using Verse.Systems;

namespace Verse.API.Models {
    public sealed class LoadedTileProvider : TileProvider {
        private TileList<Tile> _tiles = new TileList<Tile>();
        private TileList<TileObject> _tileObjects = new TileList<TileObject>();
        private TileList<ScriptableTileObject> _scriptableTileObjects = new TileList<ScriptableTileObject>();

        public LoadedTileProvider(string roomName) {
            RoomName = roomName;
            _tiles.AddRange(WorldLoader.GetTileMap(RoomName));
            _tileObjects.AddRange(WorldLoader.GetThingMap(RoomName));
            _scriptableTileObjects.AddRange(WorldLoader.GetScriptableThings(RoomName));
        }

        public LoadedTileProvider(TileProvider provider) {
            RoomName = provider.RoomName;
            _tiles.AddRange(provider.GetTiles());
            _tileObjects.AddRange(provider.GetTileObjects());
            _scriptableTileObjects.AddRange(provider.GetScriptableTileObjects());
        }

        public override Tile GetTileAt(TilePosition position) {
            return _tiles.Get(position);
        }

        public override TileObject GetTileObjectAt(TilePosition position) {
            return _tileObjects.Get(position);
        }

        public override ScriptableTileObject GetScriptableTileObjectAt(TilePosition position) {
            return _scriptableTileObjects.Get(position);
        }

        public override void RemoveTile(Tile tile) {
            _tiles.Remove(tile);
        }

        public override void RemoveTileAt(TilePosition position) {
            _tiles.RemoveAt(position);
        }

        public override void RemoveTileObject(TileObject tile) {
            _tileObjects.Remove(tile);
        }

        public override void RemoveTileObjectAt(TilePosition position) {
            _tileObjects.RemoveAt(position);
        }

        public override void RemoveScriptableTileObject(ScriptableTileObject tile) {
            _scriptableTileObjects.Remove(tile);
        }

        public override void RemoveScriptableTileObjectAt(TilePosition position) {
            _scriptableTileObjects.RemoveAt(position);
        }

        public override bool TileOccupied(TilePosition position) {
            return _tiles.Occupied(position);
        }

        public override bool TileObjectOccupied(TilePosition position) {
            return _tileObjects.Occupied(position);
        }

        public override bool ScriptableTileObjectOccupied(TilePosition position) {
            return _scriptableTileObjects.Occupied(position);
        }

        public override List<Tile> GetTiles() {
            return _tiles.GetAll();
        }

        public override List<TileObject> GetTileObjects() {
            return _tileObjects.GetAll();
        }

        public override List<ScriptableTileObject> GetScriptableTileObjects() {
            return _scriptableTileObjects.GetAll();
        }
    }
}