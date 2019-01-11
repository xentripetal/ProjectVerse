using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Verse.API.Interfaces;
using Verse.API.Models;
using Verse.Utilities;

namespace Verse.API.Models {
    /// <summary>
    /// Tiles are the core component to all physical objects in the Verse. A standard tile is simply a
    /// terrain/ground asset that the player walks on top of.
    /// </summary>
    public class TileDef {
        /// <value>The combined name of the package and the tile name. E.g. core.barrel.</value>
        public string FullName { get; protected set; }

        /// <value>The tile name. E.g. 'barrel'.</value>
        [JsonIgnore] public readonly string Name;

        /// <value>
        /// The providing package for the Tile. For example, the Object 'core.staticobjects.deco.barrel'
        /// will return 'core.staticobjects.deco'
        /// </value>
        [JsonIgnore] public readonly string Package;

        /// <value>
        /// The top level package for the Tile. For example, all non-modded content will have the Provider 'core'.
        /// </value>
        [JsonIgnore] public readonly string Provider;

        [JsonIgnore]
        public TilePosition[] OccupiedPositions { get; protected set; }

        /// <value>Information on the Tiles sprite</value>
        public SpriteInfo SpriteInfo { get; protected set; }

        [JsonConstructor]
        public TileDef(String fullName, SpriteInfo spriteInfo) {
            FullName = fullName;
            var splitFullName = fullName.Split('.');
            Name = splitFullName.Last();
            Package = String.Join(".", splitFullName.DropLast().ToArray());
            Provider = splitFullName.First();
            SpriteInfo = spriteInfo;
            OccupiedPositions = CalculateOccupiedPositions(spriteInfo);
        }

        //Todo: Use pivot point in calculations
        TilePosition[] CalculateOccupiedPositions(SpriteInfo spriteInfo) {
            var positions = new List<TilePosition>();
            int width = Mathf.CeilToInt(spriteInfo.sprite.rect.width / spriteInfo.PixelsPerUnit);
            int height = Mathf.CeilToInt(spriteInfo.sprite.rect.height / spriteInfo.PixelsPerUnit);
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    positions.Add(new TilePosition(x, y));
                }
            }

            return positions.ToArray();
        }
    }

    /// <summary>
    /// ThingDefs are definitions are static objects in the world. They exist in 2.5 dimensions as the
    /// player can be in front of or behind them.
    /// </summary>
    public class TileObjectDef : TileDef {
        /// <value>Is true if the object has collisions enabled.</value>
        public readonly bool IsCollidable;

        /// <value>Is true if the object will go transparent when the player is behind it.</value>
        public readonly bool IsTransparentOnPlayerBehind;

        [JsonConstructor]
        public TileObjectDef(String fullName, SpriteInfo spriteInfo, bool isCollidable,
            bool isTransparentOnPlayerBehind) :
            base(fullName, spriteInfo) {
            IsCollidable = isCollidable;
            IsTransparentOnPlayerBehind = isTransparentOnPlayerBehind;
            if (spriteInfo.ColliderShape.Length > 0) {
                OccupiedPositions = CalculateOccupiedPositions(spriteInfo);
            }
        }

        // Based on Nathan Mercers complex polygon point determination algorithm.
        TilePosition[] CalculateOccupiedPositions(SpriteInfo spriteInfo) {
            var minMax = GetMinMaxPositions(spriteInfo.ColliderShape);
            var min = minMax.Item1.CurrentTilePosition;
            var max = minMax.Item2.CurrentTilePosition;

            return GetTilePositionInCollider(spriteInfo.ColliderShape, min, max);
        }

        private TilePosition[] GetTilePositionInCollider(Position[] positions, TilePosition min, TilePosition max) {
            var containedPositions = new List<TilePosition>();
            var centerOffset = new Position(.5f, .5f);

            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var pos = new Position(x, y);
                    if (pointInPolygon(pos + centerOffset, positions)) {
                        containedPositions.Add(pos.CurrentTilePosition);
                    }
                }
            }

            return containedPositions.ToArray();
        }

        private bool pointInPolygon(Position point, Position[] corners) {
            var comparingCorner = corners.Last();
            var isOdd = false;

            foreach (var corner in corners) {
                if ((corner.y < point.y && comparingCorner.y >= point.y)
                    || (comparingCorner.y < point.y && corner.y >= point.y)
                    && (corner.x <= point.x || comparingCorner.x <= point.x)) {
                    if (corner.x + (point.y - corner.y) / (comparingCorner.y - corner.y) *
                        (comparingCorner.x - corner.x) <
                        point.x) {
                        isOdd = !isOdd;
                    }
                }

                comparingCorner = corner;
            }

            return isOdd;
        }

        private (Position, Position) GetMinMaxPositions(Position[] positions) {
            var min = Position.Max;
            var max = Position.Min;
            foreach (var position in positions) {
                if (position.x > max.x) {
                    max = new Position(position.x, max.y);
                }

                if (position.x < min.x) {
                    min = new Position(position.x, min.y);
                }

                if (position.y > max.y) {
                    max = new Position(max.x, position.y);
                }

                if (position.y < min.y) {
                    min = new Position(min.x, position.y);
                }
            }

            return (min, max);
        }
    }

    public class ScriptableTileObjectDef : TileObjectDef {
        /// <value>
        /// True if the collider should act as a trigger.
        /// Must be true if using a script inheriting from ITrigger. IsCollidable must also be true
        /// </value>
        public readonly bool IsTrigger;

        /// <value>the list of scripts for the object.</value>
        [JsonIgnore] public readonly IThingScript[] Scripts;

        /// <value>List of scripts by script name only.</value>
        public readonly string[] ScriptNames;

        [JsonConstructor]
        public ScriptableTileObjectDef(String fullName, SpriteInfo spriteInfo, bool isCollidable,
            bool isTransparentOnPlayerBehind, bool isTrigger,
            string[] scriptNames) : base(fullName, spriteInfo, isCollidable, isTransparentOnPlayerBehind) {
            IsTrigger = isTrigger;
            ScriptNames = scriptNames;
            Scripts = scriptNames.Select(scriptName => ScriptAtlas.GetScript(scriptName)).ToArray();
        }
    }
}