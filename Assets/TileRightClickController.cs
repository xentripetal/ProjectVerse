using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileRightClickController : MonoBehaviour {
    public GameObject DisplayDialog;

    private bool isOpen;
    private EventSystem _eventSystem;
    private Camera _cam;

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

    void Start() {
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

    void Update() {
        if (!UIEditorState.Instance.IsTileSelected) {
            if (isOpen) {
                CloseDialog();
            }

            return;
        }


        if (isOpen && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) &&
            !_eventSystem.IsPointerOverGameObject()) {
            CloseDialog();
            return;
        }

        if (!Input.GetMouseButtonDown(1)) {
            return;
        }

        OpenDialog();
        transform.position = Input.mousePosition;
    }
}