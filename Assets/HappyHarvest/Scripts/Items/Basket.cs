using UnityEngine;

namespace HappyHarvest
{
    [CreateAssetMenu(menuName = "2D Farming/Items/Basket")]
    public class Basket : Item
    {
        public override bool CanUse(Vector3Int target)
        {
            var data = GameManager.Instance.Terrain.GetCropDataAt(target);
            return data != null && data.GrowingCrop != null && Mathf.Approximately(data.GrowthRatio, 1.0f);
        }

        public override bool Use(Vector3Int target)
        {
            var data = GameManager.Instance.Terrain.GetCropDataAt(target);
            if (!GameManager.Instance.Player.CanFitInInventory(data.GrowingCrop.Produce,
                    data.GrowingCrop.ProductPerHarvest))
                return false;
            
            var product = GameManager.Instance.Terrain.HarvestAt(target);

            if (product != null)
            {
                for (int i = 0; i < product.ProductPerHarvest; ++i)
                {
                    GameManager.Instance.Player.AddItem(product.Produce);
                }
               
                return true;
            }

            return false;
        }
    }
}