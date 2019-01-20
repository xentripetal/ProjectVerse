using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.API.Models.JSON;

namespace Verse.API.Models {
    public class LoadedTileProvider : TileProvider {
        private Dictionary<TileLayer, TileList> _tiles;

        public LoadedTileProvider(SerializableRoom sRoom, Room room) {
            _tiles = null;
            Room = room;
            LoadSerializedTiles(sRoom);
        }

        private void LoadSerializedTiles(SerializableRoom sRoom) {
            var layerDict = new Dictionary<TileLayer, List<TileUnified>>();
            var layers = new List<TileLayer>();
            foreach (var sLayer in sRoom.Layers) {
                var layer = (TileLayer) Activator.CreateInstance(sLayer.LayerType);
                layers.Add(layer);
                layerDict.Add(layer, sLayer.Tiles.Select(tile => tile.ToTile(Room, layer)).ToList());
            }
        }

        public override Room Room { get; protected set; }
        public override List<TileLayer> TileLayers { get; protected set; }

        public override List<TileUnified> GetAll(TileLayer layer) {
            return _tiles[layer].GetAll();
        }

        public override TileUnified GetAt(Vector2Int position, TileLayer layer) {
            return _tiles[layer].Get(position);
        }

        public override TileUnified GetAtOrDefault(Vector2Int position, TileLayer layer) {
            return _tiles[layer].GetOrDefault(position);
        }

        public override List<TileUnified> GetTilesWithEntities(TileLayer layer) {
            return _tiles[layer].GetAllWithTileEntities();
        }

        public override List<TileUnified> GetTilesWithEntities() {
            return _tiles.Values.SelectMany(layer => layer.GetAllWithTileEntities()).ToList();
        }

        public override bool Remove(TileUnified tile) {
            if (tile.Room != Room) {
                Debug.LogError(String.Format(
                    "The provided tile \'{0}\' at {1} for room {2} does not belong to room {3}", tile.Definition.Name,
                    tile.Position, tile.Room.Name, Room.Name));
            }

            if (!_tiles.ContainsKey(tile.TileLayer)) {
                Debug.LogError(String.Format("TileProvider for room {0} does not contain the TileLayer {1}.", Room.Name,
                    tile.TileLayer.Name));
                return false;
            }

            return _tiles[tile.TileLayer].Remove(tile);
        }

        public override bool RemoveAt(Vector2Int position, TileLayer layer) {
            if (!_tiles.ContainsKey(layer)) {
                Debug.LogError(String.Format("TileProvider for room {0} does not contain the TileLayer {1}.", Room.Name,
                    layer.Name));
                return false;
            }

            return _tiles[layer].RemoveAt(position);
        }

        public override void Add(TileUnified tile) {
            if (tile.Room != Room) {
                Debug.LogError(String.Format(
                    "The provided tile \'{0}\' at {1} for room {2} does not belong to room {3}", tile.Definition.Name,
                    tile.Position, tile.Room.Name, Room.Name));
                return;
            }

            if (!_tiles.ContainsKey(tile.TileLayer)) {
                _tiles.Add(tile.TileLayer, new TileList());
            }

            _tiles[tile.TileLayer].Add(tile);
        }
    }
}