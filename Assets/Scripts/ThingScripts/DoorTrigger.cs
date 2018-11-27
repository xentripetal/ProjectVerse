using System;
using Unity.Mathematics;
using UnityEngine.Networking.NetworkSystem;

public class DoorTriggerData : IThingData{
    public string room = "TEST!";
    public float x = 1f;
    public float y = 1f;

    public DoorTriggerData(string room, float x, float y) {
        this.room = room;
        this.x = x;
        this.y = y;
    }
}

public class DoorTrigger : IThingScript, ITrigger { 
    public Type DataModel {
        get { return typeof(DoorTriggerData); }
    }
    
    public void OnPlayerEnter(PlayerReadOnly player, IThingData data) {
        DoorTriggerData dat = (DoorTriggerData) data;
        player.RequestRoomChange(dat.room, new float2(dat.x, dat.y));
    }
}
