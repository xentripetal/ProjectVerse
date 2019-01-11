using Verse.API.Models;

namespace Verse.API {
    public abstract class Player {
        public static Player Main;

        public virtual Position Position { get; protected set; }
        public virtual Position PositionDelta { get; set; }
        public string CurrentRoom { get; protected set; }

        public float Height;
        public SpriteInfo SpriteInfo;

        public float RunSpeed;
        public float WalkSpeed;

        public bool IsMoving;
        public bool IsRunning;
        public Position CurrentInputAxis;

        public abstract void TeleportPlayer(Position position);
        public abstract void ChangeRoom(string room, Position position);
    }
}