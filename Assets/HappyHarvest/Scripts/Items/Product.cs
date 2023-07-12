using UnityEngine;

namespace HappyHarvest
{
    [CreateAssetMenu(menuName = "2D Farming/Items/Product")]
    public class Product : Item
    {
        public int SellPrice = 1;
        
        public override bool CanUse(Vector3Int target)
        {
            return true;
        }

        public override bool Use(Vector3Int target)
        {
            return true;
        }

        public override bool NeedTarget()
        {
            return false;
        }
    }
}
