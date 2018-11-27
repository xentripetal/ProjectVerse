using System;
using Unity.Mathematics;
using UnityEngine.Networking.NetworkSystem;

public class TestTriggerData : IThingData{
    public string room = "TEST!";

    public TestTriggerData(string room) {
        this.room = room;
    }
}

public class TestTrigger : IThingScript { 
    public Type DataModel {
        get { return typeof(TestTriggerData); }
    }
    
    void OnPlayerEnter(PlayerReadOnly player, TestTriggerData data) {
        player.RequestRoomChange(data.room, float2.zero);
    }
}
