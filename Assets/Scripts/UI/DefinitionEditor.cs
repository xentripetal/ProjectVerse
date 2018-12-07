using System;
using System.Runtime.ConstrainedExecution;
using UI;
using UnityEngine;
using Verse.API.Models;

public class DefinitionEditor : MonoBehaviour {

    private DefinitionEditorVisualLogicHandler _visualLogicHandler;

    private void Awake() {
        _visualLogicHandler = GetComponent<DefinitionEditorVisualLogicHandler>();
    }

    public void LoadDefinition(String definition) {
        var def = getDef(definition);
        var data = getUITileData(def);
        _visualLogicHandler.LoadValues(data);
    }

    private UITileData getUITileData(TileDef def) {
        if (def.GetType() == typeof(ScriptableThingDef)) {
            return (ScriptableThingDef) def;
        }
        if (def.GetType() == typeof(ThingDef)) {
            return (ThingDef) def;
        }
        return def;
    }

    private TileDef getDef(string definition) {
        return ObjectAtlas.getDef(definition);
    }

}
