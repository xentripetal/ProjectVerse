using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Verse.API.Models;

public class LineController : MonoBehaviour {
    public Material material;

    public GameObject VertexObject;
    public Sprite LineSprite;
    public float DraggingTolerance = .035f;
    public float NewVertexTolerance = .5f;
    
    public Color DefaultColor = Color.white;
    public Color HoverColor = Color.magenta;
    public Color DraggingColor = Color.red;
    public Color NewVertexColor = Color.blue;

    private Dictionary<int, GameObject> verticeMap = new Dictionary<int, GameObject>();
    private LineDrawer _lineDrawer;
    private List<Vector2> _positions;
    private bool isDragging = false;
    private int draggingPosition = 0;
    private GameObject _newVertex;
    private Vector2 offset;

    private static Vector2[] defaultPos = new[] {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1)
    };

    // Start is called before the first frame update
    void Awake() {
        var info = ObjectAtlas.getDef("core.static.barrel").SpriteInfo;
        _lineDrawer = GameObject.FindWithTag("LineDrawer").GetComponent<LineDrawer>();
        _positions = LoadPreviousShape(info, false);
        _newVertex = CreateNewVertexGameObject();
        LoadSprite(info);

        UpdateLineRenderer(_positions);
        //UpdateVerticeMap();
    }

    private void UpdateLineRenderer(List<Vector2> positions) {
        Image image = GetComponent<Image>();
        Vector3[] corners = new Vector3[4];
        image.rectTransform.GetWorldCorners(corners);
        var bottomleft = corners[0];
        var pivot = bottomleft + (Vector3) image.sprite.pivot / image.sprite.pixelsPerUnit; 
        _lineDrawer.SetOffset(pivot);
        _lineDrawer.SetPositions(positions);
        Debug.Log(image.rectTransform.rect.height);
        var scale = image.rectTransform.rect.height / image.preferredHeight;
        Debug.Log(scale);
    }

    #region Initialization

    private LineRenderer BuildLineRenderer() {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.sortingOrder = 5;
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.loop = true;
        lineRenderer.material = material;
        return lineRenderer;
    }

    private GameObject CreateNewVertexGameObject() {
        var newVertex = SimplePool.Spawn(VertexObject, Vector2.zero, quaternion.identity);
        newVertex.transform.parent = transform;
        newVertex.GetComponent<SpriteRenderer>().color = NewVertexColor;
        newVertex.SetActive(false);
        return newVertex;
    }

    private void LoadSprite(SpriteInfo info) {
        var sprite = Utils.InfoToSprite(info);
        offset = sprite.bounds.center;
        GetComponent<Image>().sprite = sprite;
    }

    private List<Vector2> LoadPreviousShape(SpriteInfo info, bool usePhysicsShape) {
        List<Vector2> positions;

        if (usePhysicsShape) {
            if (info.ColliderShape != null) {
                positions = info.ColliderShape.Select(pos => (Vector2) pos).ToList();
            }
            else {
                positions = defaultPos.ToList();
            }
        }
        else {
            if (info.TransparencyShape != null) {
                positions = info.TransparencyShape.Select(pos => (Vector2) pos).ToList();
            }
            else {
                positions = defaultPos.ToList();
            }
        }

        return positions;
    }

    #endregion


    // Update is called once per frame
    void Update() {
        var mousePos = GetMousePosition();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(mousePos);
        }
        if (!isDragging) {
            //CalculateNearestVector(mousePos);
        }
        else {
            //HandleDragging(mousePos);
        }
    }

    private Vector2 GetMousePosition() {
        Vector2 mousePos = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return mousePos;
    }

    private void HandleDragging(Vector2 mousePos) {
        if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }
        else {
            verticeMap[draggingPosition].transform.localPosition = mousePos;
            _positions[draggingPosition] = mousePos;
            //TODO Update Positions
        }
    }

    private void CalculateNearestVector(Vector2 mousePos) {
        var nearestVertex = GetNearestVertex(mousePos, _positions);
        var distance = (mousePos - _positions[nearestVertex]).magnitude;
        if (distance < DraggingTolerance) {
            EnableDragging(nearestVertex);
        }
        else {
            DetermineCreateNewVertex(mousePos);
            nearestVertex = -1;
        }

        UpdateVertexColors(nearestVertex);
    }

    private void DetermineCreateNewVertex(Vector2 mousePos) {
        int matchingCandidate;
        var matchingCandidateProjection = GetNearestLineCandidate(mousePos, out matchingCandidate);

        if ((matchingCandidateProjection - mousePos).magnitude < NewVertexTolerance) {
            UpdateNewVertexGameObject(true, matchingCandidateProjection);
            if (Input.GetMouseButtonDown(0)) {
                CreateVertex(matchingCandidate, matchingCandidateProjection);
            }
        }
        else {
            UpdateNewVertexGameObject(true, matchingCandidateProjection);
        }
    }

    private void CreateVertex(int matchingCandidate,
        Vector2 matchingCandidateProjection) {
        _newVertex.SetActive(false);
        InsertBeforePosition(matchingCandidateProjection, matchingCandidate);
        isDragging = true;
        draggingPosition = matchingCandidate;
    }

    private void UpdateNewVertexGameObject(bool Active, Vector2 matchingCandidateProjection) {
        _newVertex.SetActive(Active);
        _newVertex.transform.localPosition = matchingCandidateProjection;
    }

    private Vector2 GetNearestLineCandidate(Vector2 mousePos, out int matchingCandidate) {
        var previousCandidateVector = _positions.Last();
        var minCandidateProjection = new Vector2(float.MaxValue, float.MaxValue);
        matchingCandidate = 0;
        var mousePosScaled = mousePos;
        
        for (var i = 0; i < _positions.Count; i++) {
            var currentCandidateVector = _positions[i];
            var candidateProjection = GetNearestPointOnLine(previousCandidateVector, currentCandidateVector, mousePosScaled);
            if ((candidateProjection - mousePosScaled).magnitude < (minCandidateProjection - mousePosScaled).magnitude) {
                minCandidateProjection = candidateProjection;
                matchingCandidate = i;
            }

            previousCandidateVector = currentCandidateVector;
        }

        return minCandidateProjection;
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
        if (_positions.Count == 2) {
            Debug.Log("You cannot delete the last 2 vertices.");
            return;
        }
        _positions.RemoveAt(position);
        //TODO Update Positions
        UpdateVerticeMap();
    }

    private void UpdateVerticeMap() {
        foreach (var go in verticeMap) {
            SimplePool.Despawn(go.Value);
        }

        for (var i = 0; i < _positions.Count; i++) {
            var vertex = SimplePool.Spawn(VertexObject, Vector2.zero, Quaternion.identity);
            vertex.transform.parent = transform;
            vertex.transform.localPosition= _positions[i];
            //vertex.transform.localScale = new Vector2(.5f, .5f, .5f);
            verticeMap[i] = vertex;
        }
    }

    private void InsertBeforePosition(Vector2 position, int previousVertex) {
        _positions.Insert(previousVertex, position);
        //TODO Update Positions
        UpdateVerticeMap();
    }


    private void UpdateVertexColors(int nearestVertex) {
        for (var i = 0; i < _positions.Count; i++) {
            verticeMap[i].GetComponent<SpriteRenderer>().color = DefaultColor;
        }

        if (nearestVertex != -1) {
            verticeMap[nearestVertex].GetComponent<SpriteRenderer>().color = isDragging ? DraggingColor : HoverColor;
        }
    }

    public static Vector2 GetNearestPointOnLine(Vector2 start, Vector2 end, Vector2 pnt) {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector2.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }

    private int GetNearestVertex(Vector2 targetPos, List<Vector2> vertices) {
        var minDistance = float.MaxValue;
        var nearesteVertex = -1;
        for (var i = 0; i < vertices.Count; i++) {
            var distance = (targetPos - vertices[i]).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                nearesteVertex = i;
            }
        }

        return nearesteVertex;
    }
}