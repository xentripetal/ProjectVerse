using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

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
    
    void Start() {
        for (int x = 0; x < 4; x++) {
            for (int y = 0; y < 4; y++) {
                Vector3 pos = new Vector3(x*4, y*4, 0);
                SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
            }
        }
    }

    void Update() {
        
    }
}
