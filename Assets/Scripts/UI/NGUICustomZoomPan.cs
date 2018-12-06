using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUICustomZoomPan : MonoBehaviour {
    public float ZoomScale = 1f;
    public float DragScale = .001f;
    
    public TouchType PanTouchType = TouchType.RightClick;
    public UIWidget mFG;
    
    // Start is called before the first frame update
    void Start() {
        UIEventListener fgl = UIEventListener.Get(mFG.gameObject);
        fgl.onScroll += OnScroll;
        fgl.onDrag += OnDrag;
    }

    protected void OnDrag(Vector2 delta) {
        OnDrag(gameObject, delta);
    }

    protected void OnDrag(GameObject gameObject, Vector2 delta) {
        if ((TouchType) UICamera.currentTouchID == PanTouchType) {
            transform.position = transform.position + (Vector3) delta * DragScale;
        }
    }
    
    protected void OnScroll(float delta) {
        OnScroll(gameObject, delta);
    }

    protected void OnScroll(GameObject gameObject, float delta) {
        transform.localScale += transform.localScale * delta * ZoomScale;
    }
}

public enum TouchType {
    LeftClick = -1,
    RightClick = -2,
    MiddleMouseClick = -3
};
