using UnityEngine;

namespace UI.Controllers {
    public class LayersVisibilityController : MonoBehaviour {
        public GameObject TileParent;
        public GameObject TileObjectParent;

        public void TilesLayerVisibleChanged(bool value) {
            TileParent.SetActive(value);
        }

        public void TileObjectsLayerVisibleChanged(bool value) {
            TileObjectParent.SetActive(value);
        }
    }
}