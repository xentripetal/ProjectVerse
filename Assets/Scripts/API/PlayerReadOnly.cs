using Unity.Mathematics;

public interface PlayerReadOnly {
    float2 position { get; }
    bool isMoving { get; }
    bool isRunning { get; }
}
