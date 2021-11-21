using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class CameraPanAndZoom : MonoBehaviour {
    private static readonly int[] ZoomBounds = {40, 1000};

    private Camera _cam;
    private Vector3 _initialPosition;

    private int _initialRefResolutionX;
    private int _initialRefResolutionY;

    private bool _isPanning;
    private Vector3 _lastPanPosition;
    private PixelPerfectCamera _pixelCam;

    private float _resolutionRatio;

    public int MouseButton;
    public float panSpeed = .5f;
    public float zoomSpeed = 20.0f;

    private void Awake() {
        _cam = Camera.main;
        _pixelCam = _cam.GetComponent<PixelPerfectCamera>();
        _resolutionRatio = _pixelCam.refResolutionY / (float) _pixelCam.refResolutionX;
        _initialPosition = transform.position;
        _initialRefResolutionX = _pixelCam.refResolutionX;
        _initialRefResolutionY = _pixelCam.refResolutionY;
    }

    public void ResetCamera() {
        transform.position = _initialPosition;
        _pixelCam.refResolutionX = _initialRefResolutionX;
        _pixelCam.refResolutionY = _initialRefResolutionY;
    }

    // Update is called once per frame
    private void Update() {
        if (EventSystem.current.IsPointerOverGameObject() && !_isPanning) return;

        _isPanning = false;

        if (Input.GetMouseButtonDown(MouseButton)) {
            _lastPanPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(MouseButton)) {
            PanCamera(Input.mousePosition);
            _isPanning = true;
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        ZoomCamera((int) scroll);
    }

    private void PanCamera(Vector3 newPanPosition) {
        var currentPos = _cam.ScreenToWorldPoint(newPanPosition);
        var offset = _lastPanPosition - currentPos;
        if (offset.magnitude < 0.1f) return;

        //Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        var move = new Vector3(offset.x * panSpeed, offset.y * panSpeed, 0);

        transform.Translate(move);
        _lastPanPosition = currentPos;
    }

    private void ZoomCamera(int offset) {
        if (offset == 0) return;

        var refX = Clamp(_pixelCam.refResolutionX - (int) (offset * zoomSpeed), ZoomBounds[0], ZoomBounds[1]);
        _pixelCam.refResolutionX = refX;
        _pixelCam.refResolutionY = (int) (refX * _resolutionRatio);
    }

    private int Clamp(int value, int min, int max) {
        if (value > max) return max;

        if (value < min) return min;

        return value;
    }
}