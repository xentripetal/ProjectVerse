using Verse.API.Models;

namespace Verse.API {
    public abstract class TileEntity {
        public virtual Tile Tile { get; protected set; }
    }
}