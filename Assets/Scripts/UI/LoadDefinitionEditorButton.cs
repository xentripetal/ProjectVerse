using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDefinitionEditorButton : MonoBehaviour {
    public DefinitionEditor DefinitionEditor;

    public void CreateNew() {
        
    }
    
    public void LoadTree() {
        DefinitionEditor.LoadDefinition("core.vegetation.tree");
    }

    public void LoadBarrel() {
        DefinitionEditor.LoadDefinition("core.static.barrel");
    }

    public void LoadTrigger() {
        DefinitionEditor.LoadDefinition("core.scriptable.trigger");
    }

    public void LoadGrass() {
        DefinitionEditor.LoadDefinition("core.tiles.grass");
    }
    
    public void LoadLightGrass() {
        DefinitionEditor.LoadDefinition("core.tiles.lightgrass");
    }
}
