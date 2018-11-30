using System;
using System.Linq;

namespace Verse.API.Models {
    /// <summary>
    /// Tiles are the core component to all physical objects in the Verse. A standard tile is simply a
    /// terrain/ground asset that the player walks on top of.
    /// </summary>
    public class TileDef {
        /// <value>The combined name of the package and the tile name. E.g. core.barrel.</value>
        public string FullName { get; protected set; }

        /// <value>The tile name. E.g. 'barrel'.</value>
        public readonly string Name;

        /// <value>
        /// The providing package for the Tile. For example, the Object 'core.staticobjects.deco.barrel'
        /// will return 'core.staticobjects.deco'
        /// </value>
        public readonly string Package;

        /// <value>
        /// The top level package for the Tile. For example, all non-modded content will have the Provider 'core'.
        /// </value>
        public readonly string Provider;

        /// <value>Information on the Tiles sprite</value>
        public SpriteInfo SpriteInfo { get; protected set; }

        public TileDef(String fullName, SpriteInfo spriteInfo) {
            FullName = fullName;
            var splitFullName = fullName.Split('.');
            Name = splitFullName.Last();
            Package = String.Join(".", splitFullName.DropLast().ToArray());
            Provider = splitFullName.First();
            SpriteInfo = spriteInfo;
        }
    }

    /// <summary>
    /// ThingDefs are definitions are static objects in the world. They exist in 2.5 dimensions as the
    /// player can be in front of or behind them.
    /// </summary>
    public class ThingDef : TileDef {
        /// <value>Is true if the object has collisions enabled.</value>
        public readonly bool IsCollidable;

        public ThingDef(String fullName, SpriteInfo spriteInfo, bool isCollidable) : base(fullName, spriteInfo) {
            IsCollidable = isCollidable;
        }
    }

    public class ScriptableThingDef : ThingDef {
        /// <value>
        /// True if the collider should act as a trigger.
        /// Must be true if using a script inheriting from ITrigger. IsCollidable must also be true
        /// </value>
        public readonly bool IsTrigger;

        /// <value>The list of scripts for the Object.</value>
        public readonly IThingScript[] Scripts;

        public ScriptableThingDef(String fullName, SpriteInfo spriteInfo, bool isCollidable, bool isTrigger,
            IThingScript[] scripts) : base(fullName, spriteInfo, isCollidable) {
            IsTrigger = isTrigger;
            Scripts = scripts;
        }
    }
}
