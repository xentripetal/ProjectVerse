using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.API.Models {
    public sealed class LoadedTileProvider : TileProvider {
        private readonly Dictionary<TileLayer, TileList> _tiles;

        public LoadedTileProvider(Room room) {
            _tiles = new Dictionary<TileLayer, TileList>();
            TileLayers = new List<TileLayer>();
            Room = room;
        }

        public override Room Room { get; protected set; }
        public override List<TileLayer> TileLayers { get; protected set; }

        public override List<Tile> GetAll(TileLayer layer) {
            return _tiles[layer].GetAll();
        }

        public override Tile GetAt(Vector2Int position, TileLayer layer) {
            return _tiles[layer].Get(position);
        }

        public override Tile GetAtOrDefault(Vector2Int position, TileLayer layer) {
            return _tiles[layer].GetOrDefault(position);
        }

        public override List<Tile> GetTilesWithEntities(TileLayer layer) {
            return _tiles[layer].GetAllWithTileEntities();
        }

        public override List<Tile> GetTilesWithEntities() {
            return _tiles.Values.SelectMany(layer => layer.GetAllWithTileEntities()).ToList();
        }

        public override bool Remove(Tile tile) {
            if (tile.Room != Room)
                Debug.LogError(string.Format(
                    "The provided tile \'{0}\' at {1} for room {2} does not belong to room {3}", tile.Definition.Name,
                    tile.Position, tile.Room.Name, Room.Name));

            if (!_tiles.ContainsKey(tile.Layer)) {
                Debug.LogError(string.Format("TileProvider for room {0} does not contain the TileLayer {1}.", Room.Name,
                    tile.Layer.Name));
                return false;
            }

            return _tiles[tile.Layer].Remove(tile);
        }

        public override bool RemoveAt(Vector2Int position, TileLayer layer) {
            if (!_tiles.ContainsKey(layer)) {
                Debug.LogError(string.Format("TileProvider for room {0} does not contain the TileLayer {1}.", Room.Name,
                    layer.Name));
                return false;
            }

            return _tiles[layer].RemoveAt(position);
        }

        public override void Add(Tile tile) {
            if (tile.Room != Room) {
                Debug.LogError(string.Format(
                    "The provided tile \'{0}\' at {1} for room {2} does not belong to room {3}", tile.Definition.Name,
                    tile.Position, tile.Room.Name, Room.Name));
                return;
            }

            if (!_tiles.ContainsKey(tile.Layer)) {
                _tiles.Add(tile.Layer, new TileList());
                TileLayers.Add(tile.Layer);
            }

            _tiles[tile.Layer].Add(tile);
        }
    }
}