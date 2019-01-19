using System.Collections.Generic;
using UnityEngine;

namespace Verse.API.Models {
    public class TileProvider {
        private Dictionary<TileLayer, TileUnified[]> _tiles;

        public TileProvider(Dictionary<TileLayer, TileUnified[]> tiles) {
            _tiles = tiles;
        }

        public TileUnified[] GetTiles(TileLayer layer) {
            return _tiles[layer];
        }

        public TileUnified GetTileAt(Vector2Int pos, TileLayer layer) {
            foreach (var tile in _tiles[layer]) {
                if (tile.Position == pos) {
                    return tile;
                }
            }

            return null;
        }
    }
}