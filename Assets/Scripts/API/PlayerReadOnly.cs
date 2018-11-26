using System;
using Unity.Mathematics;

public interface PlayerReadOnly {
    float2 position { get; }
    bool isMoving { get; }
    bool isRunning { get; }

    void RequestRoomChange(String room, float2 position);
}
