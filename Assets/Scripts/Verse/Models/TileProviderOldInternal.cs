namespace Verse.API.Models {
    public abstract class TileProviderOldInternal : TileProviderOld {
        public abstract void Remove(Tile tile);
        public abstract void RemoveTileAt(TilePosition position);
        public abstract void RemoveTileObjectAt(TilePosition position);
        public abstract void RemoveScriptableTileObjectAt(TilePosition position);
    }
}