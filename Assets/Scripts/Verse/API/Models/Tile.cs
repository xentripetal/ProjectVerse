using System.Collections.Generic;
using UnityEngine;
using Verse.API.Interfaces;

namespace Verse.API.Models {
    public class TileUnified {
        public TileUnifiedDef Definition { get; protected set; }
        public Vector2Int Position { get; set; }
        public Room Room { get; protected set; }
        public TileLayer TileLayer { get; protected set; }
        public TileEntity TileEntity { get; protected set; }

        public TileUnified(TileUnifiedDef definition, Vector2Int position, Room room, TileLayer layer, TileEntity
            tileEntity) {
            Definition = definition;
            Position = position;
            Room = room;
            TileLayer = layer;
            TileEntity = tileEntity;
        }
    }

    public abstract class Tile {
        protected TileDef m_TileDef { get; set; }

        public TileDef Definition {
            get => m_TileDef;
        }

        public virtual TilePosition Position { get; protected set; }
        public virtual RoomOld RoomOld { get; protected set; }

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