using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Verse.API.Models;

public class TileDefListPopulator : MonoBehaviour {
    public GameObject TileDefEntryPrefab;
    public Transform VerticalLayoutParent;
    public UIOptionalWindow WarningDialog;
    public SelectedTileDefController SelectedTileDefController;

    private bool _includeTiles = true;
    private bool _includeTileObjects = true;
    private bool _includeTileObjectEntities = true;
    private string _searchString;

    private string[] _tileDefs;
    private string[] _tileObjectDefs;
    private string[] _tileObjectEntityDefs;

    public void SetIncludeTiles(bool value) {
        _includeTiles = value;
        DisplayLists();
    }

    public void SetIncludeTileObjects(bool value) {
        _includeTileObjects = value;
        DisplayLists();
    }

    public void SetIncludeTileObjectEntities(bool value) {
        _includeTileObjectEntities = value;
        DisplayLists();
    }

    public void SetSearchString(string value) {
        _searchString = value;
        DisplayLists();
    }

    private void OnEnable() {
        BuildLists();
        DisplayLists();
    }

    public void BuildLists() {
        _tileDefs = TileAtlas.GetTileNames();
        _tileObjectDefs = TileAtlas.GetTileObjectNames();
        _tileObjectEntityDefs = TileAtlas.GetScriptableTileObjectNames();
    }

    private void TileDefSelected(string tileName) {
        var tile = TileAtlas.GetDef(tileName);
        SelectedTileDefController.TileDefSelectedInternal(tile);
    }

    private void TileDefEditClicked(string tileName) {
        WarningDialog.ShowWindow();
    }

    public void DisplayLists() {
        foreach (Transform currentDisplayedEntry in VerticalLayoutParent) {
            SimplePool.Despawn(currentDisplayedEntry.gameObject);
        }

        var filteredDefs = FilterLists();
        foreach (var defName in filteredDefs) {
            var def = TileAtlas.GetDef(defName);
            var go = SimplePool.Spawn(TileDefEntryPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(VerticalLayoutParent);
            go.transform.localScale = Vector3.one;
            go.GetComponentInChildren<TextMeshProUGUI>().text = def.FullName;
            var images = go.GetComponentsInChildren<Image>();
            foreach (var image in images) {
                if (image.gameObject.name == "Preview Image") {
                    image.sprite = def.SpriteInfo.sprite;
                }
            }

            foreach (Transform child in go.transform) {
                if (child.name == "Edit") {
                    child.GetComponent<Button>().onClick.AddListener(delegate { TileDefEditClicked(defName); });
                }

                else if (child.name == "Button") {
                    child.GetComponent<Button>().onClick.AddListener(delegate { TileDefSelected(defName); });
                }
            }
        }
    }

    private List<string> FilterLists() {
        var candidates = new List<string>();
        if (_includeTiles) {
            candidates.AddRange(_tileDefs);
        }

        if (_includeTileObjects) {
            candidates.AddRange(_tileObjectDefs);
        }

        if (_includeTileObjectEntities) {
            candidates.AddRange(_tileObjectEntityDefs);
        }

        var included = new List<string>();
        if (_searchString != null && _searchString != "") {
            foreach (var name in candidates) {
                if (name.Contains(_searchString)) {
                    included.Add(name);
                }
            }
        }
        else {
            included = candidates;
        }

        return included;
    }
}