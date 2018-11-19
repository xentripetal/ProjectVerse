using System;

public class DoorTriggerData : IThingScriptData{
    public string room;
    public float x, y;
}

public class DoorTrigger : IThingScript { 
    public Type DataModel {
        get { return typeof(DoorTriggerData); }
    }

    public DoorTrigger() {
        
    }
    
    void OnPlayerEnter() {
    }
}
