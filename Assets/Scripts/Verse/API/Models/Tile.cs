using System.Collections.Generic;
using Verse.API.Interfaces;

namespace Verse.API.Models {
    public abstract class Tile {
        protected TileDef m_TileDef { get; set; }

        public TileDef Definition {
            get => m_TileDef;
        }

        public virtual TilePosition Position { get; protected set; }
        public virtual Room Room { get; protected set; }

        public abstract void Destroy();
    }

    public abstract class TileObject : Tile {
        protected TileObjectDef m_TileObjectDef { get; set; }

        public new TileObjectDef Definition {
            get => m_TileObjectDef;
        }
    }

    public abstract class TileObjectEntity : TileObject {
        protected TileObjectEntityDef m_TileObjectEntityDef { get; set; }

        public new TileObjectEntityDef Definition {
            get => m_TileObjectEntityDef;
        }

        public virtual List<IThingData> Datasets { get; protected set; }
    }
}