using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float smoothSpeed = .125f;
    public Vector3 offset;
    private Vector3 smoothedPosition;
    
    void FixedUpdate() {
        Vector3 desiredPosition = target.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Vector3 offset = new Vector3(0, 0, 1);
        Vector3 topMiddle = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 0)) + offset;
        Vector3 bottomMiddle = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)) + offset;
        
        Gizmos.DrawLine(topMiddle, bottomMiddle);
    }
}
