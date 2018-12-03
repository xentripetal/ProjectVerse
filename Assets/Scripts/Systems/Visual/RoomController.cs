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
        [SerializeField] public GameObject TransparencyColliderPrefab;

        private ObjectAtlas ObjectAtlas;

        private Dictionary<GameObject, Thing> activeObjects;
        private Stack<GameObject> activeTerrainTiles;

        public string currentRoom { get; private set; }

        public Position TopRight { get; private set; }
        public Position BottomLeft { get; private set; }
        public Position Center { get; private set; }

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
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameObject tree = GameObject.Find("TileObject (17)");
                string json = "[";
                foreach (var pos in tree.GetComponent<PolygonCollider2D>().GetPath(0)) {
                    json += "{\"x\": " + pos.x + ",";
                    json += "\"y\": " + pos.y;
                    json += "},";
                }

                json += "]";
                Debug.Log(json);
            }
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
                foreach (Transform child in GO.transform) {
                    SimplePool.Despawn(child.gameObject);
                }

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
            UpdateCornerPositions(colliders.EdgePoints);
        }

        private void UpdateCornerPositions(IList<Position> colliderPoints) {
            var minX = colliderPoints[0].x;
            var maxX = colliderPoints[0].x;
            var minY = colliderPoints[0].y;
            var maxY = colliderPoints[0].y;
            foreach (var pos in colliderPoints) {
                minX = (pos.x < minX) ? pos.x : minX;
                maxX = (pos.x > maxX) ? pos.x : maxX;
                minY = (pos.y < minY) ? pos.y : minY;
                maxY = (pos.x > maxY) ? pos.y : maxY;
            }

            TopRight = new Position(maxX, maxY);
            BottomLeft = new Position(minY, minX);
            Center = new Position((minX + maxX) / 2, (minY + maxY) / 2);
        }

        private void BuildBoxColliders(IList<BoxColliderInfo> boxColliders) {
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


            Sprite sprite = Utils.InfoToSprite(currentThingDef.SpriteInfo);
            poolGo.GetComponent<SpriteRenderer>().sprite = sprite;
            var collider = poolGo.GetComponent<PolygonCollider2D>();
            collider.enabled = currentThingDef.IsCollidable;
            collider.isTrigger = false;
            if (currentThingDef.IsCollidable) {
                UpdateColliderPaths(collider, currentThingDef.SpriteInfo.ColliderShape);
            }

            if (currentThingDef.IsTransparentOnPlayerBehind) {
                if (currentThingDef.SpriteInfo.TransparencyShape != null) {
                    GameObject transparencyGo =
                        SimplePool.Spawn(TransparencyColliderPrefab, pos, Quaternion.identity);
                    UpdateColliderPaths(transparencyGo.GetComponent<PolygonCollider2D>(),
                        currentThingDef.SpriteInfo.TransparencyShape);
                    transparencyGo.transform.parent = poolGo.transform;
                }
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

        private void UpdateColliderPaths(PolygonCollider2D collider, Position[] paths) {
            EmptyPreviousColliders(collider);

            if (paths != null) {
                CopyNewPaths(collider, paths);
            }
        }

        private void CopyNewPaths(PolygonCollider2D collider, Position[] paths) {
            collider.SetPath(0,
                paths.Select(point => (Vector2) point).ToArray());
        }

        private void EmptyPreviousColliders(PolygonCollider2D collider) {
            for (int i = 0; i < collider.pathCount; i++) {
                collider.SetPath(i, null);
            }
        }

        #endregion
    }
}