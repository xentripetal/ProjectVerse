using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        buildRoom("main");
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            switch (currentRoom) {
                case "main":
                    changeRoom("test");
                    break;
                case "test":
                    changeRoom("main");
                    break;
            }
        }
    }

    public void changeRoom(string room) {
        destroyRoom();
        buildRoom(room);
    }

    public Thing GetThingFromGameObject(GameObject GO) {
        return activeObjects[GO];
    }

    void destroyRoom() {
        foreach (GameObject GO in activeObjects.Keys) {
            SimplePool.Despawn(GO);
        }

        foreach (GameObject GO in activeTerrainTiles) {
            SimplePool.Despawn(GO);
        }

        activeObjects = new Dictionary<GameObject, Thing>();
    }

    void buildRoom(string room) {
        currentRoom = room;

        buildTempTerrainAtlas();

        GameObject poolGO;
        foreach (TileJson tile in getTerrainMap(room).Tiles) {
            Vector3 pos = new Vector3(tile.X, tile.Y, 0);
            poolGO = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = terrainAtlas[tile.name].sprite;
            poolGO.transform.parent = TerrainRoot.transform;
            activeTerrainTiles.Push(poolGO);
        }

        ThingDef currentThingDef;
        Thing currentThing;
        foreach (Thing thing in getObjectMap(room)) {
            if (thing.Datasets != null) {
                Debug.Log(thing.Datasets[0].GetType().FullName);
            }

            currentThingDef = thing.Definition;
            Vector3 pos = new Vector3(thing.Position.x, thing.Position.y, thing.Position.y * Utils.zPositionMultiplier + Utils.zPositionOffset);
            poolGO = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = currentThingDef.Sprite;
            var collider = poolGO.AddComponent<PolygonCollider2D>();
            collider.isTrigger = currentThingDef.isTrigger;
            poolGO.transform.parent = ObjectRoot.transform;
            currentThing = new Thing(currentThingDef, Utils.SwapVectorDimension(pos), null);
            activeObjects[poolGO] = currentThing;
        }
    }

    private TileMap getTerrainMap(string room) {
        TextAsset jsonObj = Resources.Load<TextAsset>("Rooms/" + room + "/TerrainMap");
        return JsonUtility.FromJson<TileMap>(jsonObj.text);
    }

    private IList<Thing> getObjectMap(string room) {
        var jsonString = Resources.Load<TextAsset>("Rooms/" + room + "/ObjectMap").text;
        var settings  = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        var serializableThings = JsonConvert.DeserializeObject<List<SerializableThing>>(jsonString, settings);
        var objectMap = new List<Thing>();
        foreach (var sThing in serializableThings) {
            objectMap.Add(new Thing(sThing));
        }
        
        return objectMap;
    }

    private void buildTempTerrainAtlas() {
        TerrainTile grass = new TerrainTile("core.grass", "Sprites/Terrain/grass4x4tile");
        TerrainTile grass2 = new TerrainTile("core.grass2", "Sprites/Terrain/grass2-4x4tile");
        terrainAtlas = new Dictionary<string, TerrainTile>();
        terrainAtlas.Add(grass.name, grass);
        terrainAtlas.Add(grass2.name, grass2);
    }
}