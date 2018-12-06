using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LineDrawer : MonoBehaviour {
    public GameObject LineGameobject;
    public float Width;
    public bool Loop;
    public Transform LineParent;

    private Stack<GameObject> lines;

    // Start is called before the first frame update
    void Awake() {
        lines = new Stack<GameObject>();
    }

    public void SetScale(Vector2 scale) {
        LineParent.localScale = scale;
    }

    public void SetOffset(Vector2 offset) {
        LineParent.position = offset;
    }

    public void SetPositions(List<Vector2> positions) {
        if (positions.Count < 2) {
            Debug.LogError("LineDrawer requires atleast 2 positions.");
            return;
        }
        DrawLines(positions);
    }

    void DrawLines(List<Vector2> positions) {
        EmptyPreviousLines();
        
        var prevPos = positions[0];
        for (int i = 1; i < positions.Count; i++) {
            var pos = positions[i];
            CreateLine(pos, prevPos);
            prevPos = pos;
        }

        if (Loop) {
           CreateLine(positions[0], prevPos); 
        }
    }

    void CreateLine(Vector2 pos1, Vector2 pos2) {
        var angle = Mathf.Atan2(pos1.y-pos2.y, pos1.x-pos2.x)*180 / Mathf.PI;
        var angleVector = new Vector3(0, 0, angle);
        var center = (pos1 + pos2) / 2;
        var gameObject = SimplePool.Spawn(LineGameobject, center, Quaternion.Euler(angleVector));
        gameObject.transform.parent = LineParent;
        gameObject.transform.localPosition = center;
        var distance = (pos1 - pos2).magnitude;
        var scale = new Vector3(distance, Width, gameObject.transform.localScale.z);
        gameObject.transform.localScale = scale;
        lines.Push(gameObject);
    }

    void EmptyPreviousLines() {
        if (lines == null) {
            lines = new Stack<GameObject>();
        }
        foreach (var gameObject in lines) {
            SimplePool.Despawn(gameObject);
        }
    }
    
}
