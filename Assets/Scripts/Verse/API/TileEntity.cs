using Verse.API.Models;

namespace Verse.API {
    public abstract class TileEntity {
        public abstract Tile Tile { get; protected set; }
    }
}