using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

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
    private Thing GetThingFromGameObject(GameObject GO) {
        return roomController.GetThingFromGameObject(GO);
    }
    
    private T[] GetScriptsImplementingInterface<T>(Thing thing) {
        return GetScriptsImplementingInterface<T>(thing.Definition.Scripts);
    }
    
    private T[] GetScriptsImplementingInterface<T>(IThingScript[] scripts) {
        return scripts.OfType<T>().ToArray();
    }

    private IThingData GetDatasetOfType(Thing thing, Type dataType) {
        foreach (IThingData thingData in thing.Datasets) {
            if (dataType.IsAssignableFrom(thingData.GetType())) {
                return thingData;
            }
        }
        Debug.Log("No dataType " + dataType + " was found");
        return null;
    }
    #endregion

    #region ITrigger
    public void OnPlayerEnter(Player player, GameObject GO) {
        Thing thing = GetThingFromGameObject(GO);
        ITrigger[] triggerScripts = GetScriptsImplementingInterface<ITrigger>(thing);
        foreach (ITrigger trigger in triggerScripts) {
            IThingData dataset = GetDatasetOfType(thing, trigger.DataModel);
            trigger.OnPlayerEnter(player, dataset);
        }
    }
    #endregion
}
