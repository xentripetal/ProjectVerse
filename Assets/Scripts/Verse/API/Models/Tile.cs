using System.Collections.Generic;
using Verse.API.Interfaces;

namespace Verse.API.Models {
    public class Tile {
        public TileDef Definition { get; }
        public TilePosition TilePosition { get; }

        public Tile(TileDef definition, TilePosition tilePosition) {
            Definition = definition;
            TilePosition = tilePosition;
        }
    }

    public class TileObject : Tile {
        public new readonly TileObjectDef Definition;

        public TileObject(TileObjectDef definition, TilePosition tilePosition) : base(definition, tilePosition) {
            Definition = definition;
        }
    }

    public class ScriptableTileObject : TileObject {
        public new readonly ScriptableTileObjectDef Definition;
        public IList<IThingData> Datasets { get; }

        public ScriptableTileObject(ScriptableTileObjectDef definition, TilePosition tilePosition, IList<IThingData> datasets) : base(
            definition, tilePosition) {
            Definition = definition;
            Datasets = datasets;
        }
    }
}