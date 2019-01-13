using System.Collections.Generic;
using Verse.API.Interfaces;
using Verse.Systems;

namespace Verse.API.Models {
    public sealed class TileActual : Tile {
        public override TilePosition Position { get; protected set; }

        public override Room Room { get; protected set; }

        public TileActual(TileDef definition, TilePosition position, Room room) {
            m_TileDef = definition;
            Position = position;
            Room = room;
        }

        public override void Destroy() {
            var apiController = ApiController.Instance;
            apiController.OnTileDestroyExclusive(this);
            apiController.OnTileDestroy(this);
        }
    }

    public sealed class TileObjectActual : TileObject {
        public override TilePosition Position { get; protected set; }
        public override Room Room { get; protected set; }

        public TileObjectActual(TileObjectDef definition, TilePosition position, Room room) {
            m_TileDef = definition;
            m_TileObjectDef = definition;
            Position = position;
            Room = room;
        }

        public override void Destroy() {
            var apiController = ApiController.Instance;
            apiController.OnTileObjectDestroyExclusive(this);
            apiController.OnTileObjectDestroy(this);
            apiController.OnTileDestroy(this);
        }
    }

    public sealed class TileObjectEntityActual : TileObjectEntity {
        public override TilePosition Position { get; protected set; }
        public override Room Room { get; protected set; }
        public override List<IThingData> Datasets { get; protected set; }

        public TileObjectEntityActual(TileObjectEntityDef definition, TilePosition position, Room room,
            List<IThingData> datasets) {
            m_TileDef = definition;
            m_TileObjectDef = definition;
            m_TileObjectEntityDef = definition;
            Position = position;
            Room = room;
            Datasets = datasets;
        }

        public override void Destroy() {
            ApiController.Instance.OnTileObjectEntityDestroy(this);
            ApiController.Instance.OnTileObjectDestroy(this);
            ApiController.Instance.OnTileDestroy(this);
        }
    }
}