using System;
using System.Collections.Generic;
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
        for (int x = 0; x < 4; x++) {
            for (int y = 0; y < 4; y++) {
                Vector3 pos = new Vector3(x*4, y*4, 0);
                poolGO = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
                poolGO.GetComponent<SpriteRenderer>().sprite = terrainAtlas["core.grass"].sprite;
                poolGO.transform.parent = TerrainRoot.transform;
            }
        }

        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 2; y++) {
                Vector3 pos = new Vector3(x*4, y*4, y*4*Utils.zPositionMultiplier + Utils.zPositionOffset);
                poolGO = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
                poolGO.GetComponent<SpriteRenderer>().sprite = objectAtlas["core.barrel"].sprite;
                poolGO.AddComponent<PolygonCollider2D>();
                poolGO.transform.parent = ObjectRoot.transform;
            }
        }
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
