using System;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;

public class Player : PlayerReadOnly
{
    public float2 position { get; set; }
    public bool isMoving { get; set; }
    public bool isRunning { get; set; }
    public string currentRoom { get; set; }

    public Player() {
        position = float2.zero;
    }

    public void RequestRoomChange(String room, float2 pos) {
       Debug.Log("Room change requested to " + room + " at position " + pos); 
    }
}
