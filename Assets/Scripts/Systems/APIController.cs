using System;
using System.Linq;
using UnityEngine;
using Verse.API.Models;
using Verse.Systems.Visual;

public class APIController : MonoBehaviour {
    private RoomController roomController;

    public static APIController Instance;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        roomController = RoomController.Instance;
    }

    #region Utilities
    private ScriptableThing GetScriptableThingFromGameObject(GameObject GO) {
        return roomController.GetScriptableThingFromGameObject(GO);
    }
    
    private T[] GetScriptsImplementingInterface<T>(ScriptableThing thing) {
        if (thing.Datasets == null) {
            return new T[] { };
        }
        return GetScriptsImplementingInterface<T>(thing.Definition.Scripts);
    }
    
    private T[] GetScriptsImplementingInterface<T>(IThingScript[] scripts) {
        return scripts.OfType<T>().ToArray();
    }

    private IThingData GetDatasetOfType(ScriptableThing thing, Type dataType) {
        foreach (IThingData thingData in thing.Datasets) {
            if (dataType.IsInstanceOfType(thingData)) {
                return thingData;
            }
        }
        Debug.Log("No dataType " + dataType + " was found");
        return null;
    }
    #endregion

    #region ITrigger
    public void OnPlayerEnter(Player player, GameObject GO) {
        var thing = GetScriptableThingFromGameObject(GO);
        ITrigger[] triggerScripts = GetScriptsImplementingInterface<ITrigger>(thing);
        foreach (ITrigger trigger in triggerScripts) {
            IThingData dataset = GetDatasetOfType(thing, trigger.DataModel);
            trigger.OnPlayerEnter(player, dataset);
        }
    }
    #endregion
}
