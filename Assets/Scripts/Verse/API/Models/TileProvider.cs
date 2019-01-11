using System.Collections.Generic;

namespace Verse.API.Models {
    public abstract class TileProvider {
        public virtual string RoomName { get; protected set; }
        public abstract Tile GetTileAt(TilePosition position);

        public virtual Tile GetTileAtOrDefault(TilePosition position) {
            if (!TileOccupied(position)) {
                return null;
            }

            return GetTileAt(position);
        }

        public abstract TileObject GetTileObjectAt(TilePosition position);

        public virtual TileObject GetTileObjectAtOrDefault(TilePosition position) {
            if (!TileObjectOccupied(position)) {
                return null;
            }

            return GetTileObjectAt(position);
        }

        public abstract ScriptableTileObject GetScriptableTileObjectAt(TilePosition position);

        public virtual ScriptableTileObject GetScriptableTileObjectAtOrDefault(TilePosition position) {
            if (!ScriptableTileObjectOccupied(position)) {
                return null;
            }

            return GetScriptableTileObjectAt(position);
        }

        public virtual TileObject GetOptionalScriptableTileObject(TilePosition position) {
            var tileObject = (TileObject) GetScriptableTileObjectAtOrDefault(position);
            if (tileObject == null) {
                tileObject = GetTileObjectAt(position);
            }

            return tileObject;
        }

        public virtual TileObject GetOptionalScriptableTileObjectOrDefault(TilePosition position) {
            var tileObject = (TileObject) GetScriptableTileObjectAtOrDefault(position);
            if (tileObject == null) {
                tileObject = GetTileObjectAtOrDefault(position);
            }

            return tileObject;
        }

        public abstract void RemoveTile(Tile tile);
        public abstract void RemoveTileAt(TilePosition position);
        public abstract void RemoveTileObject(TileObject tile);
        public abstract void RemoveTileObjectAt(TilePosition position);
        public abstract void RemoveScriptableTileObject(ScriptableTileObject tile);
        public abstract void RemoveScriptableTileObjectAt(TilePosition position);

        public abstract bool TileOccupied(TilePosition position);
        public abstract bool TileObjectOccupied(TilePosition position);
        public abstract bool ScriptableTileObjectOccupied(TilePosition position);

        public virtual bool CanPlaceObjectAt(TilePosition position) {
            return TileOccupied(position) && !TileObjectOccupied(position) && !ScriptableTileObjectOccupied(position);
        }

        public abstract List<Tile> GetTiles();
        public abstract List<TileObject> GetTileObjects();
        public abstract List<ScriptableTileObject> GetScriptableTileObjects();
    }
}