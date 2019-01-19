using System.Collections.Generic;
using Verse.API.Interfaces;
using Verse.Systems;

namespace Verse.API.Models {
    public sealed class TileActual : Tile {
        public override TilePosition Position { get; protected set; }

        public override RoomOld RoomOld { get; protected set; }

        public TileActual(TileDef definition, TilePosition position, RoomOld roomOld) {
            m_TileDef = definition;
            Position = position;
            RoomOld = roomOld;
        }

        public override void Destroy() {
            var apiController = ApiController.Instance;
            apiController.OnTileDestroyExclusive(this);
            apiController.OnTileDestroy(this);
        }
    }

    public sealed class TileObjectActual : TileObject {
        public override TilePosition Position { get; protected set; }
        public override RoomOld RoomOld { get; protected set; }

        public TileObjectActual(TileObjectDef definition, TilePosition position, RoomOld roomOld) {
            m_TileDef = definition;
            m_TileObjectDef = definition;
            Position = position;
            RoomOld = roomOld;
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
        public override RoomOld RoomOld { get; protected set; }
        public override List<IThingData> Datasets { get; protected set; }

        public TileObjectEntityActual(TileObjectEntityDef definition, TilePosition position, RoomOld roomOld,
            List<IThingData> datasets) {
            m_TileDef = definition;
            m_TileObjectDef = definition;
            m_TileObjectEntityDef = definition;
            Position = position;
            RoomOld = roomOld;
            Datasets = datasets;
        }

        public override void Destroy() {
            ApiController.Instance.OnTileObjectEntityDestroy(this);
            ApiController.Instance.OnTileObjectDestroy(this);
            ApiController.Instance.OnTileDestroy(this);
        }
    }
}