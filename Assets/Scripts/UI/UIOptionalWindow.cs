using UnityEngine;

public class UIOptionalWindow : MonoBehaviour {
    // Start is called before the first frame update
    private void Awake() { }

    public void SetWindowActive(bool active) {
        gameObject.SetActive(active);
    }

    public void ShowWindow() {
        gameObject.SetActive(true);
    }

    public void HideWindow() {
        gameObject.SetActive(false);
    }
}