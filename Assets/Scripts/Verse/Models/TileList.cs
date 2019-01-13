using System.Collections.Generic;

namespace Verse.API.Models {
    public class TileList<T> where T : Tile {
        private List<List<T>> _positionList;
        private List<T> _uniqueList;

        public TileList() {
            _positionList = new List<List<T>>();
            _uniqueList = new List<T>();
        }

        public TileList(int x, int y) {
            _positionList = new List<List<T>>(x);
            foreach (var list in _positionList) {
                list.Capacity = y;
            }

            _uniqueList = new List<T>(x * y);
        }

        public T Get(TilePosition position) {
            return _positionList[position.x][position.y];
        }

        public T GetOrDefault(TilePosition position) {
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

        public bool Contains(T tile) {
            return _uniqueList.Contains(tile);
        }

        public bool Occupied(TilePosition position) {
            return _positionList.Count > position.x
                   && _positionList[position.x] != null
                   && _positionList[position.x].Count > position.y
                   && _positionList[position.x][position.y] != null;
        }

        public List<T> GetAll() {
            return new List<T>(_uniqueList);
        }

        public void AddRange(IEnumerable<T> tiles) {
            foreach (var tile in tiles) {
                Add(tile);
            }
        }

        public void Remove(T tile) {
            _uniqueList.Remove(tile);
            foreach (var pos in tile.Definition.OccupiedPositions) {
                var occupiedPos = tile.Position + pos;
                _positionList[occupiedPos.x][occupiedPos.y] = null;
            }
        }

        public void RemoveAt(TilePosition pos) {
            Remove(_positionList[pos.x][pos.y]);
        }

        public void Add(T tile) {
            _uniqueList.Add(tile);
            foreach (var pos in tile.Definition.OccupiedPositions) {
                var occupiedPos = tile.Position + pos;
                var tileAtOccupiedPos = GetOrDefault(occupiedPos);
                if (tileAtOccupiedPos != null) {
                    tileAtOccupiedPos.Destroy();
                }

                if (_positionList.Count <= occupiedPos.x) {
                    for (int i = _positionList.Count; i <= occupiedPos.x; i++) {
                        _positionList.Add(new List<T>());
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