using System.Collections.Generic;
using Verse.Systems;

namespace Verse.API.Models {
    public sealed class LoadedTileProvider : TileProviderInternal {
        private TileList<Tile> _tiles = new TileList<Tile>();
        private TileList<TileObject> _tileObjects = new TileList<TileObject>();
        private TileList<TileObjectEntity> _scriptableTileObjects = new TileList<TileObjectEntity>();


        public LoadedTileProvider(Room room) {
            Room = room;
            _tiles.AddRange(WorldLoader.GetTileMap(room));
            _scriptableTileObjects.AddRange(WorldLoader.GetScriptableThings(room));
            _tileObjects.AddRange(WorldLoader.GetThingMap(room));
        }

        public LoadedTileProvider(TileProvider provider) {
            Room = provider.Room;
            _tiles.AddRange(provider.GetTiles());
            _tileObjects.AddRange(provider.GetTileObjects());
            _scriptableTileObjects.AddRange(provider.GetTileObjectEntities());
        }

        //todo reconsider how to handle calling api controller
        public override void Add(Tile tile) {
            if (tile.Definition.TileType == TileType.TileObjectEntity) {
                TileObjectEntity entityTile = (TileObjectEntity) tile;
                _scriptableTileObjects.Add(entityTile);
                ApiController.Instance.OnTileObjectEntityCreated(entityTile);
                ApiController.Instance.OnTileObjectCreated(entityTile);
            }
            else if (tile.Definition.TileType == TileType.TileObject) {
                TileObject tileObject = (TileObject) tile;
                _tileObjects.Add(tileObject);
                ApiController.Instance.OnTileObjectCreated(tileObject);
                ApiController.Instance.OnTileObjectCreatedExclusive(tileObject);
            }
            else {
                _tiles.Add(tile);
                ApiController.Instance.OnTileCreatedExclusive(tile);
            }

            ApiController.Instance.OnTileCreated(tile);
        }

        public override Tile GetTileAt(TilePosition position) {
            return _tiles.Get(position);
        }

        public override TileObject GetTileObjectAt(TilePosition position) {
            return _tileObjects.Get(position);
        }

        public override TileObjectEntity GetScriptableTileObjectAt(TilePosition position) {
            return _scriptableTileObjects.Get(position);
        }

        public override void Remove(Tile tile) {
            if (typeof(TileObjectEntity).IsInstanceOfType(tile)) {
                _scriptableTileObjects.Remove((TileObjectEntity) tile);
            }
            else if (typeof(TileObject).IsInstanceOfType(tile)) {
                _tileObjects.Remove((TileObject) tile);
            }
            else {
                _tiles.Remove(tile);
            }
        }

        public override void RemoveTileAt(TilePosition position) {
            _tiles.RemoveAt(position);
        }

        public override void RemoveTileObjectAt(TilePosition position) {
            _tileObjects.RemoveAt(position);
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

        public override List<TileObjectEntity> GetTileObjectEntities() {
            return _scriptableTileObjects.GetAll();
        }
    }
}