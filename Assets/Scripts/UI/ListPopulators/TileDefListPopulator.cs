using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Verse.API.Models;

public class TileDefListPopulator : MonoBehaviour {
    private bool _includeTiles = true;
    private string _searchString;

    private List<string> _tileDefs;
    public SelectedTileDefController SelectedTileDefController;
    public GameObject TileDefEntryPrefab;
    public Transform VerticalLayoutParent;
    public UIOptionalWindow WarningDialog;

    public void SetSearchString(string value) {
        _searchString = value;
        DisplayLists();
    }

    private void OnEnable() {
        BuildLists();
        DisplayLists();
    }

    public void BuildLists() {
        _tileDefs = TileDefMap.GetKeys();
    }

    private void TileDefSelected(string tileName) {
        var tile = TileDefMap.GetTileDef(tileName);
        SelectedTileDefController.TileDefSelectedInternal(tile);
    }

    private void TileDefEditClicked(string tileName) {
        WarningDialog.ShowWindow();
    }

    public void DisplayLists() {
        foreach (Transform currentDisplayedEntry in VerticalLayoutParent)
            SimplePool.Despawn(currentDisplayedEntry.gameObject);

        var filteredDefs = FilterLists();
        foreach (var defName in filteredDefs) {
            var def = TileDefMap.GetTileDef(defName);
            var go = SimplePool.Spawn(TileDefEntryPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(VerticalLayoutParent);
            go.transform.localScale = Vector3.one;
            go.GetComponentInChildren<TextMeshProUGUI>().text = def.Name;
            var images = go.GetComponentsInChildren<Image>();
            foreach (var image in images)
                if (image.gameObject.name == "Preview Image")
                    image.sprite = def.Sprite;

            foreach (Transform child in go.transform)
                if (child.name == "Edit")
                    child.GetComponent<Button>().onClick.AddListener(delegate { TileDefEditClicked(defName); });

                else if (child.name == "Button")
                    child.GetComponent<Button>().onClick.AddListener(delegate { TileDefSelected(defName); });
        }
    }

    private List<string> FilterLists() {
        var candidates = _tileDefs;

        var included = new List<string>();
        if (_searchString != null && _searchString != "")
            foreach (var name in candidates)
                if (name.Contains(_searchString))
                    included.Add(name);
        else
            included = candidates;

        return included;
    }
}