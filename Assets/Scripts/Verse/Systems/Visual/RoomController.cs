using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.API.Models;
using Verse.Utilities;

namespace Verse.Systems.Visual {
    public class RoomController : MonoBehaviour {
        public GameObject TerrainRoot;
        public GameObject ObjectRoot;
        public GameObject TerrainTilePrefab;
        public GameObject ObjectTilePrefab;
        public GameObject TransparencyColliderPrefab;

        public Dictionary<GameObject, Tile> activeTiles;
        public Dictionary<Tile, GameObject> activeTilesReverse;

        public string CurrentRoomName { get; private set; }
        public Room CurrentRoom { get; private set; }

        public bool HasActiveRoom { get; private set; }

        public Position TopRight { get; private set; }
        public Position BottomLeft { get; private set; }
        public Position Center { get; private set; }

        public static RoomController Instance;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            activeTiles = new Dictionary<GameObject, Tile>();
            activeTilesReverse = new Dictionary<Tile, GameObject>();
            BuildRoom("main");
        }

        public void ChangeRoom(string newRoom, string oldRoom) {
            DestroyRoom();
            BuildRoom(newRoom);
        }

        public void DestroyRoom() {
            foreach (var go in activeTiles.Keys) {
                foreach (Transform child in go.transform) {
                    SimplePool.Despawn(child.gameObject);
                }

                SimplePool.Despawn(go);
            }


            activeTiles = new Dictionary<GameObject, Tile>();
            activeTilesReverse = new Dictionary<Tile, GameObject>();
            CurrentRoomName = "";
            CurrentRoom = null;
            HasActiveRoom = false;
        }

        private void BuildRoom(string roomKey) {
            CurrentRoomName = roomKey;
            var room = RoomMap.GetRoom(CurrentRoomName);
            CurrentRoom = room;
            HasActiveRoom = true;

            BuildColliders(room.Colliders);
            foreach (var layer in CurrentRoom.Tiles.TileLayers) {
                foreach (var tile in CurrentRoom.Tiles.GetAll(layer)) {
                    BuildTile(tile);
                }
            }
        }

        void BuildTile(Tile tile) {
            var tileDef = tile.Definition;
            var pos = tile.Layer.TilePositionToVisualPosition(tile.Position);

            GameObject poolGo = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);

            var spriteRenderer = poolGo.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = tileDef.Sprite;
            spriteRenderer.sortingOrder = tile.Layer.SortingOrder;

            if (tileDef.HasCollision) {
                var colliderComponent = poolGo.AddComponent<PolygonCollider2D>();
                colliderComponent.isTrigger = false;
            }


            poolGo.transform.parent = ObjectRoot.transform;
            activeTiles.Add(poolGo, tile);
            activeTilesReverse.Add(tile, poolGo);
        }


        #region Collider Construction

        private void BuildColliders(RoomColliders colliders) {
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
            if (boxColliders == null) {
                return;
            }

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
                currentComponent.offset = ApiMappings.Vector2FromPosition(boxColliders[i].Position);
                currentComponent.size = ApiMappings.Vector2FromPosition(boxColliders[i].Size);
            }
        }

        private void BuildEdgeColliders(IList<Position> colliderPoints) {
            EdgeCollider2D colliderRoot = TerrainRoot.GetComponent<EdgeCollider2D>();
            colliderRoot.points = colliderPoints.Select(pos => ApiMappings.Vector2FromPosition(pos)).ToArray();
        }

        #endregion

        #region Tile Construction

/**
        private void BuildTile(Tile tile) {
            Vector3 pos = new Vector3(tile.Position.x, tile.Position.y, 0);
            GameObject poolGo = SimplePool.Spawn(TerrainTilePrefab, pos, Quaternion.identity);
            poolGo.GetComponent<SpriteRenderer>().sprite = ApiMappings.InfoToSprite(tile.Definition.SpriteInfo);
            poolGo.transform.parent = TerrainRoot.transform;
            activeTerrainTiles.Add(poolGo, tile);
            activeTerrainTilesReverse.Add(tile, poolGo);
        }

        private void BuildTileObject(TileObject tileObject) {
            var currentThingDef = tileObject.Definition;
            var pos = GetLayeredPosition(tileObject.Position);

            GameObject poolGo = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);


            Sprite sprite = ApiMappings.InfoToSprite(currentThingDef.SpriteInfo);
            poolGo.GetComponent<SpriteRenderer>().sprite = sprite;
            var colliderComponent = poolGo.GetComponent<PolygonCollider2D>();
            colliderComponent.enabled = currentThingDef.IsCollidable;
            colliderComponent.isTrigger = false;
            if (currentThingDef.IsCollidable) {
                UpdateColliderPaths(colliderComponent, currentThingDef.SpriteInfo.ColliderShape);
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
            activeTiles.Add(poolGo, tileObject);
            activeTilesReverse.Add(tileObject, poolGo);
        }

        private void BuildTileObjectEntity(TileObjectEntity tileObjectEntity) {
            var currentThingDef = tileObjectEntity.Definition;
            var pos = new Vector3(tileObjectEntity.Position.x, tileObjectEntity.Position.y,
                tileObjectEntity.Position.y * Constants.ZPositionMultiplier + Constants.ZPositionOffset);
            var poolGo = SimplePool.Spawn(ObjectTilePrefab, pos, Quaternion.identity);
            poolGo.GetComponent<SpriteRenderer>().sprite = ApiMappings.InfoToSprite(currentThingDef.SpriteInfo);
            var colliderComponent = poolGo.GetComponent<PolygonCollider2D>();
            colliderComponent.enabled = currentThingDef.IsCollidable;
            if (currentThingDef.IsCollidable) {
                UpdateColliderPaths(colliderComponent, currentThingDef.SpriteInfo.ColliderShape);
                colliderComponent.isTrigger = currentThingDef.IsTrigger;
            }

            poolGo.transform.parent = ObjectRoot.transform;
            activeTiles.Add(poolGo, tileObjectEntity);
            activeTilesReverse.Add(tileObjectEntity, poolGo);
        }

        private Vector3 GetLayeredPosition(TilePosition tilePosition) {
            return new Vector3(tilePosition.x, tilePosition.y,
                tilePosition.y * Constants.ZPositionMultiplier + Constants.ZPositionOffset);
        } **/

        private void UpdateColliderPaths(PolygonCollider2D colliderComponent, Position[] paths) {
            EmptyPreviousColliders(colliderComponent);

            if (paths != null) {
                CopyNewPaths(colliderComponent, paths);
            }
        }

        private void CopyNewPaths(PolygonCollider2D colliderComponent, Position[] paths) {
            colliderComponent.SetPath(0,
                paths.Select(point => ApiMappings.Vector2FromPosition(point)).ToArray());
        }

        private void EmptyPreviousColliders(PolygonCollider2D colliderComponent) {
            for (var i = 0; i < colliderComponent.pathCount; i++) {
                colliderComponent.SetPath(i, null);
            }
        }

        #endregion
    }
}