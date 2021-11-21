using System.Collections.Generic;
using UnityEngine;

namespace Verse.API.Models {
    public abstract class TileProvider {
        public abstract Room Room { get; protected set; }
        public abstract List<TileLayer> TileLayers { get; protected set; }
        public abstract List<Tile> GetAll(TileLayer layer);
        public abstract Tile GetAt(Vector2Int position, TileLayer layer);
        public abstract Tile GetAtOrDefault(Vector2Int position, TileLayer layer);
        public abstract List<Tile> GetTilesWithEntities(TileLayer layer);
        public abstract List<Tile> GetTilesWithEntities();
        public abstract bool Remove(Tile tile);
        public abstract bool RemoveAt(Vector2Int position, TileLayer layer);
        public abstract void Add(Tile tile);
    }
}