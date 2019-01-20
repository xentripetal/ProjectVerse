using System.Collections.Generic;
using UnityEngine;

namespace Verse.API.Models {
    public class TileList {
        private List<List<TileUnified>> _positionList;
        private List<TileUnified> _uniqueList;
        private List<TileUnified> _entityList;

        public TileList() {
            _positionList = new List<List<TileUnified>>();
            _uniqueList = new List<TileUnified>();
            _entityList = new List<TileUnified>();
        }


        public TileUnified Get(Vector2Int position) {
            return _positionList[position.x][position.y];
        }

        public TileUnified GetOrDefault(Vector2Int position) {
            if (_positionList.Count <= position.x) {
                return null;
            }

            var yList = _positionList[position.x];
            if (yList == null) {
                return null;
            }

            if (yList.Count <= position.y) {
                return null;
            }

            return yList[position.y];
        }

        public bool Contains(TileUnified tile) {
            return _uniqueList.Contains(tile);
        }

        public bool Occupied(Vector2Int position) {
            return _positionList.Count > position.x
                   && _positionList[position.x] != null
                   && _positionList[position.x].Count > position.y
                   && _positionList[position.x][position.y] != null;
        }

        public List<TileUnified> GetAll() {
            return new List<TileUnified>(_uniqueList);
        }

        public List<TileUnified> GetAllWithTileEntities() {
            return new List<TileUnified>(_entityList);
        }

        public void AddRange(IEnumerable<TileUnified> tiles) {
            foreach (var tile in tiles) {
                Add(tile);
            }
        }

        public bool Remove(TileUnified tile) {
            var exists = _uniqueList.Remove(tile);
            if (!exists) {
                return false;
            }

            if (tile.TileEntity != null) {
                _entityList.Remove(tile);
            }

            foreach (var pos in tile.Definition.OccupiedPositions) {
                var occupiedPos = tile.Position + pos;
                _positionList[occupiedPos.x][occupiedPos.y] = null;
            }

            return true;
        }

        public bool RemoveAt(Vector2Int pos) {
            var tile = GetOrDefault(pos);
            if (tile == null) {
                return false;
            }

            return Remove(tile);
        }

        public void Add(TileUnified tile) {
            _uniqueList.Add(tile);
            if (tile.TileEntity != null) {
                _entityList.Add(tile);
            }

            foreach (var pos in tile.Definition.OccupiedPositions) {
                var occupiedPos = tile.Position + pos;
                var tileAtOccupiedPos = GetOrDefault(occupiedPos);
                if (tileAtOccupiedPos != null) {
                    //Destrot
                }

                if (_positionList.Count <= occupiedPos.x) {
                    for (int i = _positionList.Count; i <= occupiedPos.x; i++) {
                        _positionList.Add(new List<TileUnified>());
                    }
                }

                var tilesXList = _positionList[occupiedPos.x];
                if (tilesXList.Count <= occupiedPos.y) {
                    for (int i = tilesXList.Count; i <= occupiedPos.y; i++) {
                        tilesXList.Add(null);
                    }
                }

                tilesXList[occupiedPos.y] = tile;
            }
        }
    }
}