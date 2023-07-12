using UnityEngine;

namespace HappyHarvest
{
    [CreateAssetMenu(fileName = "Hoe", menuName = "2D Farming/Items/Hoe")]
    public class Hoe : Item
    {
        public override bool CanUse(Vector3Int target)
        {
            return GameManager.Instance?.Terrain != null && GameManager.Instance.Terrain.IsTillable(target);
        }

        public override bool Use(Vector3Int target)
        {
            GameManager.Instance.Terrain.TillAt(target);
            return true;
        }
    }
}
