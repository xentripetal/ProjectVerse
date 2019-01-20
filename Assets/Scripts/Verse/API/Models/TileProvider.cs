using System.Collections.Generic;
using UnityEngine;

namespace Verse.API.Models {
    public abstract class TileProvider {
        public abstract Room Room { get; protected set; }
        public abstract List<TileLayer> TileLayers { get; protected set; }
        public abstract List<TileUnified> GetAll(TileLayer layer);
        public abstract TileUnified GetAt(Vector2Int position, TileLayer layer);
        public abstract TileUnified GetAtOrDefault(Vector2Int position, TileLayer layer);
        public abstract List<TileUnified> GetTilesWithEntities(TileLayer layer);
        public abstract List<TileUnified> GetTilesWithEntities();
        public abstract bool Remove(TileUnified tile);
        public abstract bool RemoveAt(Vector2Int position, TileLayer layer);
        public abstract void Add(TileUnified tile);
    }
}