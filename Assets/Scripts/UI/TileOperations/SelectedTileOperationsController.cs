using UI;
using UnityEngine;

public class SelectedTileOperationsController : MonoBehaviour {
    private void Awake() { }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl)) {
            TileOperationsHandler.Undo();
            return;
        }

        if (!UIEditorState.Instance.IsTileSelected) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Delete)) {
            var tile = UIEditorState.Instance.CurrentSelectedTile;
            TileOperationsHandler.DestroyTile(tile);
        }
    }
}