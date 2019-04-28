using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileRightClickController : MonoBehaviour {
    private Camera _cam;
    private EventSystem _eventSystem;
    public GameObject DisplayDialog;

    private bool isOpen;

    public void Delete() {
        CloseDialog();
        TileOperationsHandler.DestroyTile(UIEditorState.Instance.CurrentSelectedTile);
    }

    public void SelectDef() {
        CloseDialog();
        Debug.Log("Select");
    }

    public void EditDef() {
        CloseDialog();
        Debug.Log("Edit Def");
    }

    public void EditData() {
        CloseDialog();
        Debug.Log("Edit Data");
    }

    private void Start() {
        _eventSystem = EventSystem.current;
        _cam = Camera.main;
    }

    private void OpenDialog() {
        isOpen = true;
        DisplayDialog.SetActive(true);
    }

    private void CloseDialog() {
        isOpen = false;
        DisplayDialog.SetActive(false);
    }

    private void Update() {
        if (!UIEditorState.Instance.IsTileSelected) {
            if (isOpen) CloseDialog();

            return;
        }


        if (isOpen && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) &&
            !_eventSystem.IsPointerOverGameObject()) {
            CloseDialog();
            return;
        }

        if (!Input.GetMouseButtonDown(1)) return;

        OpenDialog();
        transform.position = Input.mousePosition;
    }
}