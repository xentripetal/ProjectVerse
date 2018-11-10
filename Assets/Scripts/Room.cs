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
 
    void Start() {
        buildTempTerrainAtlas();
        buildTempObjectAtlas();
        
        GameObject poolGO;
        foreach (TileJson tile in getTerrainMap().Tiles) {
            Vector3 pos = new Vector3(tile.X, tile.Y, 0);
            poolGO = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = terrainAtlas[tile.name].sprite;
            poolGO.transform.parent = TerrainRoot.transform;
        }

        foreach (TileJson tile in getObjectMap().Tiles) {
            Vector3 pos = new Vector3(tile.X, tile.Y, tile.Y*Utils.zPositionMultiplier + Utils.zPositionOffset);
            poolGO = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            poolGO.GetComponent<SpriteRenderer>().sprite = objectAtlas[tile.name].sprite;
            poolGO.AddComponent<PolygonCollider2D>();
            poolGO.transform.parent = ObjectRoot.transform;
        }
    }

    private TileMap getTerrainMap() {
        TextAsset jsonObj = Resources.Load<TextAsset>("TerrainMap");
        return JsonUtility.FromJson<TileMap>(jsonObj.text);
    }

    private TileMap getObjectMap() {
        TextAsset jsonObj = Resources.Load<TextAsset>("ObjectMap");
        return JsonUtility.FromJson<TileMap>(jsonObj.text);
    }

    private void buildTempTerrainAtlas() {
        TerrainTile grass = new TerrainTile("core.grass", "Sprites/Terrain/grass4x4tile");
        terrainAtlas = new Dictionary<string, TerrainTile>();
        terrainAtlas.Add(grass.name, grass);
    }
    
    private void buildTempObjectAtlas() {
        Thing barrel = new Thing("core.barrel", "Sprites/Objects/barrel");
        objectAtlas = new Dictionary<string, Thing>();
        objectAtlas.Add(barrel.name, barrel);
    }
}
