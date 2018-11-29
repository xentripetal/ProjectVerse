using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using API.Models;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomController : MonoBehaviour {
    [SerializeField] public GameObject TerrainRoot;

    [SerializeField] public GameObject ObjectRoot;

    [SerializeField] public GameObject TerrainTilePrefab;

    [SerializeField] public GameObject ObjectTilePrefab;

    private Dictionary<String, TerrainTile> terrainAtlas;
    private ObjectAtlas ObjectAtlas;

    private Dictionary<GameObject, Thing> activeObjects;
    private Stack<GameObject> activeTerrainTiles;

    private string currentRoom;

    public static RoomController Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        activeObjects = new Dictionary<GameObject, Thing>();
        activeTerrainTiles = new Stack<GameObject>();
        BuildRoom("main");
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            switch (currentRoom) {
                case "main":
                    ChangeRoom("test");
                    break;
                case "test":
                    ChangeRoom("main");
                    break;
            }
        }
    }

    public void ChangeRoom(string room) {
        DestroyRoom();
        BuildRoom(room);

    }

    public Thing GetThingFromGameObject(GameObject GO) {
        return activeObjects[GO];
    }

    void DestroyRoom() {
        foreach (GameObject GO in activeObjects.Keys) {
            SimplePool.Despawn(GO);
        }

        foreach (GameObject GO in activeTerrainTiles) {
            SimplePool.Despawn(GO);
        }

        activeObjects = new Dictionary<GameObject, Thing>();
    }

    void BuildRoom(string room) {
        currentRoom = room;

        BuildTempTerrainAtlas();

        TerrainMap currentTerrainMap = GetTerrainMap(currentRoom);
        BuildEdgeColliders(currentTerrainMap.Colliders.EdgePoints);
        BuildBoxColliders(currentTerrainMap.Colliders.BoxColliders);
        
        foreach (var tile in currentTerrainMap.Tiles) {
            BuildTile(tile);
        }
        
        foreach (Thing thing in GetObjectMap(currentRoom)) {
            BuildThing(thing);
        }
    }

    private void BuildBoxColliders(IList<BoxCollider> boxColliders) {
        IList<BoxCollider2D> colliderComponents = TerrainRoot.GetComponents<BoxCollider2D>().ToList();
        var diff = boxColliders.Count - colliderComponents.Count; 
        if (diff > 0) {
            for (int i = 0; i < diff; i++) {
                colliderComponents.Add(TerrainRoot.AddComponent<BoxCollider2D>());
            }
        } else if (diff < 0) {
            for (int i = colliderComponents.Count - 1; i >= boxColliders.Count; i--) {
                Destroy(colliderComponents[i]);
                colliderComponents.RemoveAt(i);
            }
        }

        for (int i = 0; i < boxColliders.Count; i++) {
            var currentComponent = colliderComponents[i];
            currentComponent.offset = boxColliders[i].Position;
            currentComponent.size = boxColliders[i].Size;
        }
    }
    
    private void BuildEdgeColliders(IList<Position> colliderPoints) {
        EdgeCollider2D colliderRoot = TerrainRoot.GetComponent<EdgeCollider2D>();
        colliderRoot.points = colliderPoints.Select(pos => (Vector2) pos).ToArray();
    }

    private void BuildTile(SerializableTile tile) {
        Vector3 pos = (Vector3) tile.Position;
        GameObject poolGO = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
        poolGO.GetComponent<SpriteRenderer>().sprite = terrainAtlas[tile.Definition].sprite;
        poolGO.transform.parent = TerrainRoot.transform;
        activeTerrainTiles.Push(poolGO);
    }

    private void BuildThing(Thing thing) {
        var currentThingDef = thing.Definition;
        Vector3 pos = new Vector3(thing.Position.x, thing.Position.y, thing.Position.y * Utils.zPositionMultiplier + Utils.zPositionOffset);
        GameObject poolGO = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
        poolGO.GetComponent<SpriteRenderer>().sprite = currentThingDef.Sprite;
        var collider = poolGO.AddComponent<PolygonCollider2D>();
        collider.isTrigger = currentThingDef.isTrigger;
        poolGO.transform.parent = ObjectRoot.transform;
        activeObjects[poolGO] = thing;
    }

    private TerrainMap GetTerrainMap(string room) {
        var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/TerrainMap").text;
        var settings  = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        TerrainMap terrainMap = JsonConvert.DeserializeObject<TerrainMap>(jsonString, settings);
        if (terrainMap.Colliders.BoxColliders == null) {
            terrainMap.Colliders.BoxColliders = new List<BoxCollider>();
        }
        return terrainMap;
    }

    private IList<Thing> GetObjectMap(string room) {
        var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/ObjectMap").text;
        var settings  = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        var serializableThings = JsonConvert.DeserializeObject<List<SerializableThing>>(jsonString, settings);
        var objectMap = new List<Thing>();
        foreach (var sThing in serializableThings) {
            objectMap.Add(new Thing(sThing));
            Debug.Log(sThing.Definition);
        }
        
        return objectMap;
    }

    private void BuildTempTerrainAtlas() {
        TerrainTile grass = new TerrainTile("core.grass", "Sprites/Terrain/grass4x4tile");
        TerrainTile grass2 = new TerrainTile("core.grass2", "Sprites/Terrain/grass2-4x4tile");
        terrainAtlas = new Dictionary<string, TerrainTile>();
        terrainAtlas.Add(grass.name, grass);
        terrainAtlas.Add(grass2.name, grass2);
    }
}