using System;
using System.Collections.Generic;

public class ScriptAtlas{
    private static Dictionary<String, IThingScript> scriptAtlas;

    public static void InitializeAtlas() {
        if (scriptAtlas== null) {
            createAtlas();
        }
    }

    public static IThingScript getScript(String scriptName) {
        InitializeAtlas();
        return scriptAtlas[scriptName];
    }

    private static void createAtlas() {
        IThingScript script = new DoorTrigger();
        scriptAtlas = new Dictionary<string, IThingScript>();
        scriptAtlas.Add(script.GetType().FullName, script);
    }
    
}
