using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Unity.Mathematics;
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
        //buildRoom("main");
        List<Thing> test = new List<Thing>();
        Thing simpleThing = new Thing(ObjectAtlas.getObject("core.barrel"), float2.zero, null);
        IThingData[] dataset = new[] {new DoorTriggerData("room", 1, 1),};
        Thing triggerThing = new Thing(ObjectAtlas.getObject("core.trigger"), new float2(2, 2), dataset);
        test.Add(simpleThing);
        test.Add(triggerThing);
        string jsonString = JsonConvert.SerializeObject(test, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        Debug.Log(jsonString);
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
        foreach (TileJson tile in getObjectMap(room).Tiles) {
            if (tile.datasets != null) {
                Debug.Log(tile.datasets[0].GetType().FullName);
            }

            currentThingDef = ObjectAtlas.getObject(tile.name);
            Vector3 pos = new Vector3(tile.X, tile.Y, tile.Y * Utils.zPositionMultiplier + Utils.zPositionOffset);
            poolGO = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = currentThingDef.sprite;
            poolGO.AddComponent<PolygonCollider2D>();
            poolGO.transform.parent = ObjectRoot.transform;
            currentThing = new Thing(currentThingDef, Utils.SwapVectorDimension(pos), null);
            activeObjects[poolGO] = currentThing;
        }
    }

    private TileMap getTerrainMap(string room) {
        TextAsset jsonObj = Resources.Load<TextAsset>("Rooms/" + room + "/TerrainMap");
        return JsonUtility.FromJson<TileMap>(jsonObj.text);
    }

    private TileMap getObjectMap(string room) {
        String jsonObj = Resources.Load<TextAsset>("Rooms/" + room + "/ObjectMap").text;
        var deserializedObject = JsonConvert.DeserializeObject<TileMap>(jsonObj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        return deserializedObject;
    }

    private void buildTempTerrainAtlas() {
        TerrainTile grass = new TerrainTile("core.grass", "Sprites/Terrain/grass4x4tile");
        TerrainTile grass2 = new TerrainTile("core.grass2", "Sprites/Terrain/grass2-4x4tile");
        terrainAtlas = new Dictionary<string, TerrainTile>();
        terrainAtlas.Add(grass.name, grass);
        terrainAtlas.Add(grass2.name, grass2);
    }
}