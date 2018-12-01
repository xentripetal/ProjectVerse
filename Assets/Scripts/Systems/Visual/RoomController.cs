using System.Collections.Generic;
using System.Linq;
using Systems;
using UnityEngine;
using Verse.API.Models;

namespace Verse.Systems.Visual {
    public class RoomController : MonoBehaviour {
        [SerializeField] public GameObject TerrainRoot;
        [SerializeField] public GameObject ObjectRoot;
        [SerializeField] public GameObject TerrainTilePrefab;
        [SerializeField] public GameObject ObjectTilePrefab;

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

        public void ChangeRoom(string room) {
            DestroyRoom();
            BuildRoom(room);
        }

        public ScriptableThing GetScriptableThingFromGameObject(GameObject GO) {
            return (ScriptableThing) activeObjects[GO];
        }

        private void DestroyRoom() {
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

            TerrainMap currentTerrainMap = WorldLoader.GetTerrainMap(currentRoom);
            BuildColliders(currentTerrainMap.Colliders);

            foreach (var tile in currentTerrainMap.Tiles) {
                BuildTile(tile);
            }

            foreach (Thing thing in WorldLoader.GetThingMap(currentRoom)) {
                BuildThing(thing);
            }

            foreach (var thing in WorldLoader.GetScriptableThings(currentRoom)) {
                BuildScriptableThing(thing);
            }
        }

        #region Collider Construction

        private void BuildColliders(Colliders colliders) {
            BuildEdgeColliders(colliders.EdgePoints);
            BuildBoxColliders(colliders.BoxColliders);
        }

        private void BuildBoxColliders(IList<BoxCollider> boxColliders) {
            IList<BoxCollider2D> colliderComponents = TerrainRoot.GetComponents<BoxCollider2D>().ToList();
            var diff = boxColliders.Count - colliderComponents.Count;
            if (diff > 0) {
                for (int i = 0; i < diff; i++) {
                    colliderComponents.Add(TerrainRoot.AddComponent<BoxCollider2D>());
                }
            }
            else if (diff < 0) {
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

        #endregion

        #region Tile Construction

        private void BuildTile(Tile tile) {
            Vector3 pos = (Vector3) tile.Position;
            GameObject poolGo = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
            poolGo.GetComponent<SpriteRenderer>().sprite = Utils.InfoToSprite(tile.Definition.SpriteInfo);
            poolGo.transform.parent = TerrainRoot.transform;
            activeTerrainTiles.Push(poolGo);
        }

        private void BuildThing(Thing thing) {
            var currentThingDef = thing.Definition;
            var pos = GetLayeredPosition(thing.Position);

            GameObject poolGo = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);

            poolGo.GetComponent<SpriteRenderer>().sprite = Utils.InfoToSprite(currentThingDef.SpriteInfo);
            var collider = poolGo.GetComponent<PolygonCollider2D>();
            collider.enabled = currentThingDef.IsCollidable;
            collider.isTrigger = false;
            if (currentThingDef.IsCollidable) {
                UpdateColliderPaths(collider, currentThingDef.SpriteInfo.ColliderShape);
            }

            poolGo.transform.parent = ObjectRoot.transform;
            activeObjects[poolGo] = thing;
        }

        private void BuildScriptableThing(ScriptableThing thing) {
            var currentThingDef = thing.Definition;
            Vector3 pos = new Vector3(thing.Position.x, thing.Position.y,
                thing.Position.y * Utils.zPositionMultiplier + Utils.zPositionOffset);
            GameObject poolGo = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            poolGo.GetComponent<SpriteRenderer>().sprite = Utils.InfoToSprite(currentThingDef.SpriteInfo);
            var collider = poolGo.GetComponent<PolygonCollider2D>();
            collider.enabled = currentThingDef.IsCollidable;
            if (currentThingDef.IsCollidable) {
                UpdateColliderPaths(collider, currentThingDef.SpriteInfo.ColliderShape);
                collider.isTrigger = currentThingDef.IsTrigger;
            }

            poolGo.transform.parent = ObjectRoot.transform;
            activeObjects[poolGo] = thing;
        }

        private Vector3 GetLayeredPosition(Position position) {
            return new Vector3(position.x, position.y,
                position.y * Utils.zPositionMultiplier + Utils.zPositionOffset);
        }

        private void UpdateColliderPaths(PolygonCollider2D collider, List<Position[]> paths) {
            EmptyPreviousColliders(collider);

            if (paths != null) {
                CopyNewPaths(collider, paths);
            }
        }

        private void CopyNewPaths(PolygonCollider2D collider, List<Position[]> paths) {
            for (int i = 0; i < paths.Count; i++) {
                collider.SetPath(i,
                    paths[i].Select(point => (Vector2) point).ToArray());
            }
        }

        private void EmptyPreviousColliders(PolygonCollider2D collider) {
            for (int i = 0; i < collider.pathCount; i++) {
                collider.SetPath(i, null);
            }
        }

        #endregion
    }
}