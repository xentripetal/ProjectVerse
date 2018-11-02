using System.Collections;
using System.Collections.Generic;
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
        
        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 2; y++) {
                Vector3 pos = new Vector3(x*4, y*4, y*4*Utils.zPositionMultiplier + Utils.zPositionOffset);
                SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            }
        }
    }

}
