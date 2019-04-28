using System.Collections.Generic;
using UnityEngine;

namespace Verse.API.Models {
    public class TileList {
        private readonly List<Tile> _entityList;
        private readonly List<List<Tile>> _positionList;
        private readonly List<Tile> _uniqueList;

        public TileList() {
            _positionList = new List<List<Tile>>();
            _uniqueList = new List<Tile>();
            _entityList = new List<Tile>();
        }


        public Tile Get(Vector2Int position) {
            return _positionList[position.x][position.y];
        }

        public Tile GetOrDefault(Vector2Int position) {
            if (_positionList.Count <= position.x) return null;

            var yList = _positionList[position.x];
            if (yList == null) return null;

            if (yList.Count <= position.y) return null;

            return yList[position.y];
        }

        public bool Contains(Tile tile) {
            return _uniqueList.Contains(tile);
        }

        public bool Occupied(Vector2Int position) {
            return _positionList.Count > position.x
                   && _positionList[position.x] != null
                   && _positionList[position.x].Count > position.y
                   && _positionList[position.x][position.y] != null;
        }

        public List<Tile> GetAll() {
            return new List<Tile>(_uniqueList);
        }

        public List<Tile> GetAllWithTileEntities() {
            return new List<Tile>(_entityList);
        }

        public void AddRange(IEnumerable<Tile> tiles) {
            foreach (var tile in tiles) Add(tile);
        }

        public bool Remove(Tile tile) {
            var exists = _uniqueList.Remove(tile);
            if (!exists) return false;

            if (tile.Entity != null) _entityList.Remove(tile);

            foreach (var pos in tile.Definition.OccupiedPositions) {
                var occupiedPos = tile.Position + pos;
                _positionList[occupiedPos.x][occupiedPos.y] = null;
            }

            return true;
        }

        public bool RemoveAt(Vector2Int pos) {
            var tile = GetOrDefault(pos);
            if (tile == null) return false;

            return Remove(tile);
        }

        public void Add(Tile tile) {
            _uniqueList.Add(tile);
            if (tile.Entity != null) _entityList.Add(tile);

            foreach (var pos in tile.Definition.OccupiedPositions) {
                var occupiedPos = tile.Position + pos;
                var tileAtOccupiedPos = GetOrDefault(occupiedPos);
                if (tileAtOccupiedPos != null) {
                    //Destrot
                }

                if (_positionList.Count <= occupiedPos.x)
                    for (var i = _positionList.Count; i <= occupiedPos.x; i++)
                        _positionList.Add(new List<Tile>());

                var tilesXList = _positionList[occupiedPos.x];
                if (tilesXList.Count <= occupiedPos.y)
                    for (var i = tilesXList.Count; i <= occupiedPos.y; i++)
                        tilesXList.Add(null);

                tilesXList[occupiedPos.y] = tile;
            }
        }
    }
}