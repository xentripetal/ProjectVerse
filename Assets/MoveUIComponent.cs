using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Verse
{
    public class MoveUIComponent : UIBehaviour {
        public Vector2 ClickOffset;


        public void MoveDown() {
            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x + ClickOffset.x, pos.y + ClickOffset.y);
        }
        
        public void MoveUp() {
            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x - ClickOffset.x, pos.y - ClickOffset.y);
        }
    }
}
