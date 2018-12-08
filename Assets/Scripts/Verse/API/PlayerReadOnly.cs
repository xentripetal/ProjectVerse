using Unity.Mathematics;

namespace Verse.API {
    public interface IPlayerReadOnly {
        float2 Position { get; }
        bool IsMoving { get; }
        bool IsRunning { get; }

        void RequestRoomChange(string room, float2 position);
    }
}