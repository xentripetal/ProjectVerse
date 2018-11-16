using System;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.XR.WSA;

public class Room : MonoBehaviour
{
    [SerializeField]
    public GameObject TerrainRoot;

    [SerializeField]
    public GameObject ObjectRoot;

    [SerializeField] 
    public GameObject TerrainTilePrefab;

    [SerializeField] 
    public GameObject ObjectTilePrefab;

    private Dictionary<String, TerrainTile> terrainAtlas;
    private Dictionary<String, Thing> objectAtlas;

    private Stack<GameObject> activeObjects;

    private string currentRoom;

    private void Start() {
        activeObjects = new Stack<GameObject>();
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

    void destroyRoom() {
        foreach (GameObject GO in activeObjects) {
            SimplePool.Despawn(GO);
        }
        activeObjects = new Stack<GameObject>();
    }

    void buildRoom(string room) {
        currentRoom = room;
        
        buildTempTerrainAtlas();
        buildTempObjectAtlas();
        
        GameObject poolGO;
        foreach (TileJson tile in getTerrainMap(room).Tiles) {
            Vector3 pos = new Vector3(tile.X, tile.Y, 0);
            poolGO = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = terrainAtlas[tile.name].sprite;
            poolGO.transform.parent = TerrainRoot.transform;
            activeObjects.Push(poolGO);
        }

        foreach (TileJson tile in getObjectMap(room).Tiles) {
            Vector3 pos = new Vector3(tile.X, tile.Y, tile.Y*Utils.zPositionMultiplier + Utils.zPositionOffset);
            poolGO = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = objectAtlas[tile.name].sprite;
            poolGO.AddComponent<PolygonCollider2D>();
            poolGO.transform.parent = ObjectRoot.transform;
            activeObjects.Push(poolGO);
        }
    }
 
    private TileMap getTerrainMap(string room) {
        TextAsset jsonObj = Resources.Load<TextAsset>("Rooms/" + room + "/TerrainMap");
        return JsonUtility.FromJson<TileMap>(jsonObj.text);
    }

    private TileMap getObjectMap(string room) {
        TextAsset jsonObj = Resources.Load<TextAsset>("Rooms/" + room + "/ObjectMap");
        return JsonUtility.FromJson<TileMap>(jsonObj.text);
    }

    private void buildTempTerrainAtlas() {
        TerrainTile grass = new TerrainTile("core.grass", "Sprites/Terrain/grass4x4tile");
        TerrainTile grass2 = new TerrainTile("core.grass2", "Sprites/Terrain/grass2-4x4tile");
        terrainAtlas = new Dictionary<string, TerrainTile>();
        terrainAtlas.Add(grass.name, grass);
        terrainAtlas.Add(grass2.name, grass2);
    }
    
    private void buildTempObjectAtlas() {
        Thing barrel = new Thing("core.barrel", "Sprites/Objects/barrel");
        Thing trigger = new Thing("core.trigger", "Sprites/Terrain/Collision");
        objectAtlas = new Dictionary<string, Thing>();
        objectAtlas.Add(barrel.name, barrel);
        objectAtlas.Add(trigger.name, trigger);
    }
}
