using System.Collections.Generic;

namespace Verse.API.Models {
    public abstract class TileProviderOld {
        public virtual RoomOld RoomOld { get; protected set; }

        public abstract void Add(Tile tile);
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

        public abstract TileObjectEntity GetScriptableTileObjectAt(TilePosition position);

        public virtual TileObjectEntity GetScriptableTileObjectAtOrDefault(TilePosition position) {
            if (!ScriptableTileObjectOccupied(position)) {
                return null;
            }

            return GetScriptableTileObjectAt(position);
        }

        public virtual TileObject GetOptionalScriptableTileObject(TilePosition position) {
            var tileObject = (TileObject) GetScriptableTileObjectAtOrDefault(position);
            if (tileObject == null) {
                tileObject = GetTileObjectAtOrDefault(position);
            }

            return tileObject;
        }

        public abstract bool TileOccupied(TilePosition position);
        public abstract bool TileObjectOccupied(TilePosition position);
        public abstract bool ScriptableTileObjectOccupied(TilePosition position);

        public virtual bool CanPlaceObjectAt(TilePosition position) {
            return TileOccupied(position) && !TileObjectOccupied(position) && !ScriptableTileObjectOccupied(position);
        }

        public abstract List<Tile> GetTiles();
        public abstract List<TileObject> GetTileObjects();
        public abstract List<TileObjectEntity> GetTileObjectEntities();
    }
}