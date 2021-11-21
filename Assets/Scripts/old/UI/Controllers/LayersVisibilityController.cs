using UnityEngine;

namespace UI.Controllers {
    public class LayersVisibilityController : MonoBehaviour {
        public GameObject TileObjectParent;
        public GameObject TileParent;

        public void TilesLayerVisibleChanged(bool value) {
            TileParent.SetActive(value);
        }

        public void TileObjectsLayerVisibleChanged(bool value) {
            TileObjectParent.SetActive(value);
        }
    }
}