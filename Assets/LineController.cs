using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Verse.API.Models;

public class LineController : MonoBehaviour {
    public Material material;

    public GameObject VertexObject;
    public float DraggingTolerance = .035f;
    public float NewVertexTolerance = .5f;

    public float width = 0.01f;
    public Color DefaultColor = Color.white;
    public Color HoverColor = Color.magenta;
    public Color DraggingColor = Color.red;
    public Color NewVertexColor = Color.blue;

    private Dictionary<int, GameObject> verticeMap = new Dictionary<int, GameObject>();
    private LineRenderer _lineRenderer;
    private Vector3[] _positions;
    private bool isDragging = false;
    private int draggingPosition = 0;
    private GameObject _newVertex;
    private static Vector3[] defaultPos = new[] {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 1, 0),
        new Vector3(1, 0, 0)
    };

    // Start is called before the first frame update
    void Awake() {
        _lineRenderer = BuildLineRenderer();
        _positions = LoadPreviousShape(ObjectAtlas.getThingDef("core.static.barrel").SpriteInfo, true);
        _newVertex = CreateNewVertexGameObject();

        UpdateLineRenderer(_positions);
        UpdateVerticeMap();
    }

    private void UpdateLineRenderer(Vector3[] positions) {
        _lineRenderer.SetPositions(_positions);
    }

    #region Initialization

    private LineRenderer BuildLineRenderer() {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.useWorldSpace = false;
        lineRenderer.sortingOrder = 5;
        lineRenderer.positionCount = 8;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.loop = true;
        lineRenderer.material = material;
        return lineRenderer;
    }

    private GameObject CreateNewVertexGameObject() {
        var newVertex = SimplePool.Spawn(VertexObject, Vector3.zero, quaternion.identity);
        newVertex.GetComponent<SpriteRenderer>().color = NewVertexColor;
        newVertex.SetActive(false);
        return newVertex;
    }

    private Vector3[] LoadPreviousShape(SpriteInfo info, bool usePhysicsShape) {
        Vector3[] positions;
            
        if (usePhysicsShape) {
            if (info.ColliderShape != null) {
                positions = info.ColliderShape.Select(pos => (Vector3) pos).ToArray();
            }
            else {
                positions = defaultPos;
            }
        }
        else {
            if (info.TransparencyShape != null) {
                positions = info.ColliderShape.Select(pos => (Vector3) pos).ToArray();
            }
            else {
                positions = defaultPos;
            }
        }
        return positions;
    }

    #endregion


    // Update is called once per frame
    void Update() {
        var mousePos = GetMousePosition();

        if (!isDragging) {
            CalculateNearestVector(mousePos);
        }
        else {
            HandleDragging(mousePos);
        }
    }

    private Vector3 GetMousePosition() {
        Vector3 mousePos = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return mousePos - new Vector3(0, 0, mousePos.z);
    }

    private void HandleDragging(Vector3 mousePos) {
        if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }
        else {
            var newPos = mousePos + transform.position;
            verticeMap[draggingPosition].transform.position = newPos;
            _positions[draggingPosition] = mousePos;
            _lineRenderer.SetPosition(draggingPosition, mousePos);
        }
    }

    private void CalculateNearestVector(Vector3 mousePos) {
        var nearestVertex = GetNearestVertex(mousePos, _positions);
        var distance = (mousePos - _positions[nearestVertex]).magnitude;
        if (distance < DraggingTolerance) {
            EnableDragging(nearestVertex);
        }
        else {
            nearestVertex = DetermineCreateNewVertex(mousePos, nearestVertex);
        }

        UpdateVertexColors(nearestVertex);
    }

    private int DetermineCreateNewVertex(Vector3 mousePos, int nearestVertex) {
        int matchingCandidate;
        Vector3 matchingCandidateProjection;
        var earlyCandidate = GetNearestLineCandidate(mousePos, nearestVertex, out matchingCandidate,
            out matchingCandidateProjection);

        if ((matchingCandidateProjection - mousePos).magnitude < NewVertexTolerance) {
            UpdateNewVertexGameObject(true, matchingCandidateProjection);
            if (Input.GetMouseButtonDown(0)) {
                CreateVertex(nearestVertex, matchingCandidate, earlyCandidate, matchingCandidateProjection);
            }
        }
        else {
            UpdateNewVertexGameObject(false, Vector3.zero);
        }

        nearestVertex = -1;
        return nearestVertex;
    }

    private void CreateVertex(int nearestVertex, int matchingCandidate, int earlyCandidate,
        Vector3 matchingCandidateProjection) {
        _newVertex.SetActive(false);
        var smallestVertex = Mathf.Min(nearestVertex, matchingCandidate);
        if (nearestVertex == 0 && matchingCandidate == earlyCandidate) {
            smallestVertex = matchingCandidate;
        }

        InsertAfterPosition(matchingCandidateProjection, smallestVertex);
        isDragging = true;
        draggingPosition = smallestVertex + 1;
    }

    private void UpdateNewVertexGameObject(bool Active, Vector3 matchingCandidateProjection) {
        _newVertex.SetActive(Active);
        _newVertex.transform.position = matchingCandidateProjection + transform.position;
    }

    private int GetNearestLineCandidate(Vector3 mousePos, int nearestVertex, out int matchingCandidate,
        out Vector3 matchingCandidateProjection) {
        var earlyCandidate = nearestVertex == 0 ? _positions.Length - 1 : nearestVertex - 1;
        var lateCandidate = nearestVertex == _positions.Length - 1 ? 0 : nearestVertex + 1;
        var earlyCandidateProjection =
            GetNearestPointOnLine(_positions[earlyCandidate], _positions[nearestVertex], mousePos);
        var lateCandidateProjection =
            GetNearestPointOnLine(_positions[nearestVertex], _positions[lateCandidate], mousePos);
        matchingCandidate = (earlyCandidateProjection - mousePos).magnitude >
                            (lateCandidateProjection - mousePos).magnitude
            ? lateCandidate
            : earlyCandidate;
        matchingCandidateProjection = matchingCandidate == earlyCandidate
            ? earlyCandidateProjection
            : lateCandidateProjection;
        return earlyCandidate;
    }

    private void EnableDragging(int nearestVertex) {
        _newVertex.SetActive(false);
        if (Input.GetMouseButtonDown(0)) {
            isDragging = true;
            draggingPosition = nearestVertex;
        }
        else if (Input.GetKeyDown(KeyCode.Delete)) {
            DeletePosition(nearestVertex);
        }
    }

    private void DeletePosition(int position) {
        var newPositions = _positions.ToList();
        newPositions.RemoveAt(position);
        _positions = newPositions.ToArray();
        _lineRenderer.positionCount = _positions.Length;
        _lineRenderer.SetPositions(_positions);
        UpdateVerticeMap();
    }

    private void UpdateVerticeMap() {
        foreach (var go in verticeMap) {
            SimplePool.Despawn(go.Value);
        }

        for (var i = 0; i < _positions.Length; i++) {
            var vertex = SimplePool.Spawn(VertexObject, _positions[i] + transform.position, Quaternion.identity);
            vertex.transform.parent = transform;
            verticeMap[i] = vertex;
        }
    }

    private void InsertAfterPosition(Vector3 position, int previousVertex) {
        var newPositions = _positions.ToList();
        newPositions.Insert(previousVertex + 1, position);
        _positions = newPositions.ToArray();
        _lineRenderer.positionCount = _positions.Length;
        _lineRenderer.SetPositions(_positions);
        UpdateVerticeMap();
    }


    private void UpdateVertexColors(int nearestVertex) {
        for (var i = 0; i < _positions.Length; i++) {
            verticeMap[i].GetComponent<SpriteRenderer>().color = DefaultColor;
        }

        if (nearestVertex != -1) {
            verticeMap[nearestVertex].GetComponent<SpriteRenderer>().color = isDragging ? DraggingColor : HoverColor;
        }
    }

    public static Vector3 GetNearestPointOnLine(Vector3 start, Vector3 end, Vector3 pnt) {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }

    private int GetNearestVertex(Vector3 targetPos, Vector3[] vertices) {
        var minDistance = float.MaxValue;
        var nearesteVertex = -1;
        for (var i = 0; i < vertices.Length; i++) {
            var distance = (targetPos - vertices[i]).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                nearesteVertex = i;
            }
        }

        return nearesteVertex;
    }
}